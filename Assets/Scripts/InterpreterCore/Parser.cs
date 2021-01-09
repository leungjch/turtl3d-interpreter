using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser
{
    List<Token> TokenList; 
    int parserIndex = 0;
    // The parser 
    // Takes TokenList and builds an Abstract Syntax Tree

    // Alternate constructor to start parser at a specific point in token list
    public Parser(int index = 0)
    {
        parserIndex = index;
    }


    public void advanceIndex()
    {
        parserIndex += 1;
    }

    public Token peekCurrentToken()
    {
        return TokenList[parserIndex];
    }


    public Token peekPrevToken()
    {

        if (parserIndex-1 > 0)
        {
            return TokenList[parserIndex-1];
        }
        else 
        {
            return new Token(Token.TokenType.EOF, "EOF");
        }    
        
    }

    public Token peekNextToken()
    {
        if (parserIndex+1 < TokenList.Count)
        {
            return TokenList[parserIndex+1];
        }
        else 
        {
            return new Token(Token.TokenType.EOF, "EOF");
        }
    }
    // Bracket matching algorithm
    public List<Token> getInner(List<Token> tokensInner, ref int parserIndex)
    {
        int depth = 1;
        List<Token> inner = new List<Token>();

        while (depth > 0 && parserIndex < tokensInner.Count)
        {
            if (tokensInner[parserIndex]._tokenType == Token.TokenType.L_PAREN)
            {
                depth += 1;
            }
            else if (tokensInner[parserIndex]._tokenType == Token.TokenType.R_PAREN)
            {
                depth -= 1;
                // If bracket not well formed, return null
                if (depth <= 0)
                {
                    return inner;
                }
            }

            if (parserIndex < tokensInner.Count)
            {
                inner.Add(tokensInner[parserIndex]);
                parserIndex+=1;
            }
            else
            {
                break;
            }

        }
        return inner;
    }

    // Get function definition
    // Get all tokens until EN
    List<Token> getFunctionDefinition(List<Token> tokensInner, ref int parserIndex)
    {
        List<Token> functionDefinition = new List<Token>();
        while (parserIndex < tokensInner.Count)
        {
            // Debug.Log("PARSERINDEX" + parserIndex);


            if (tokensInner[parserIndex]._tokenType == Token.TokenType.END)
            {
                return functionDefinition;
            }
            else 
            {
                functionDefinition.Add(tokensInner[parserIndex]);
                parserIndex += 1;
            }
        }
        return functionDefinition;
    }

    public List<AbstractSyntaxTreeNode> runParser(List<Token> tokens) 
    {
        TokenList = tokens;
        List<AbstractSyntaxTreeNode> expressionList = new List<AbstractSyntaxTreeNode>();
        parserIndex = 0;

        while (parserIndex < TokenList.Count) 
        {   
            Token currentToken = peekCurrentToken(); 

            // Debug.Log("Current token: " + currentToken._literal);

            switch (currentToken._tokenType) 
            {
                // If we see a function declaration, scan rest of tokens until we hit END
                // Parse the inner expression as well
                case Token.TokenType.TO:
                    advanceIndex(); 
                    string functionName = peekCurrentToken()._literal;
                    advanceIndex();
                    List<Token> functionDef = getFunctionDefinition(TokenList, ref parserIndex);

                    // // Debugging
                    // Debug.Log("Printing function definition parsed");
                    // foreach (Token tok in functionDef)
                    // {
                    //     Debug.Log(JsonUtility.ToJson(tok, true));
                    // }
                    // Debug.Log("Done function definition parsed");


                    Parser interpretFunctionDef = new Parser(parserIndex);
                    List<AbstractSyntaxTreeNode> functionDefExp = interpretFunctionDef.runParser(functionDef);

                    expressionList.Add(
                        new FunctionDefinitionNode(functionName, functionDefExp)
                    );
                    break;


                case Token.TokenType.PRIMITIVE:
                    // First, check if primitive is supplied with argument
                    // Fetch next token (numeric argument, e.g. "20" in fd 20)
                    // Must be number
                    if (parserIndex+1 < TokenList.Count 
                        && peekNextToken()._tokenType != Token.TokenType.EOF 
                        && (peekNextToken()._tokenType == Token.TokenType.CONSTANT) || (peekNextToken()._tokenType == Token.TokenType.REPCOUNT)
                        ) 
                    {
                        Debug.Log("parserIndex: " + parserIndex);

                        Token nextToken = peekNextToken();
                        advanceIndex(); 

                        switch (nextToken._tokenType)
                        {
                            case Token.TokenType.CONSTANT:
                                expressionList.Add(
                                    new FunctionArgNode(currentToken._literal, Int32.Parse(nextToken._literal), false)
                                );
                                break;

                            case Token.TokenType.REPCOUNT:
                                expressionList.Add(
                                    new FunctionArgNode(currentToken._literal, 0, true)
                                );
                                break;
                        }

                    }
                    // Otherwise it is a primitive with no arguments
                    else
                    {
                        expressionList.Add(new FunctionNoArgNode(currentToken._literal));
                    }
                    break;

                // Repcount (in repeat)
                case Token.TokenType.REPCOUNT:
                        expressionList.Add(new RepcountNode());
                        break;
                
                case Token.TokenType.REPEAT:
                    // Fetch repeat count (repcount)
                    // Handle bracket matching
                    if (parserIndex+1 < TokenList.Count && peekNextToken()._tokenType != Token.TokenType.EOF && peekNextToken()._tokenType == Token.TokenType.CONSTANT)
                    {
                        advanceIndex(); // repcount
                        int repCount = Int32.Parse(peekCurrentToken()._literal);
                        // Debug.Log(repCount);
                        advanceIndex(); // (


                        // // if == '['
                        if (parserIndex < TokenList.Count && peekCurrentToken()._tokenType == Token.TokenType.L_PAREN )
                        {
                            advanceIndex(); // first token inside parens
                            List<Token> inner = getInner(TokenList, ref parserIndex);
                            Parser interpretInner = new Parser(parserIndex);
                            List<AbstractSyntaxTreeNode> innerExp = interpretInner.runParser(inner);
                            // Debugging
                            // Debug.Log("Printing inner node parsed");
                            // foreach (AbstractSyntaxTreeNode nd in innerExp)
                            // {
                            //     Debug.Log(JsonUtility.ToJson(nd, true));
                            // }

                            expressionList.Add(
                                new RepeatNode(repCount, innerExp)
                            );
                        }
                        else
                        {
                            break;
                        }

                    }
                    break;
                case Token.TokenType.EOF:
                    break;

                default:
                    Debug.Log("Error parsing token");          
                    break;
            }
            advanceIndex();
        }
        // Debug.Log("Printing full expression List");          
        // // Debugging
        // foreach (AbstractSyntaxTreeNode nd in expressionList)
        // {
        //     Debug.Log(JsonUtility.ToJson(nd, true));
        //     if (nd.type == AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.REPEAT)
        //     {
        //         Debug.Log("inner Node is");

        //         RepeatNode rnd = (RepeatNode) nd;
        //         foreach (AbstractSyntaxTreeNode rinner in rnd.inner)
        //         {
        //             Debug.Log(JsonUtility.ToJson(rinner, true));
        //         }
        //     }
        // }
        // Debug.Log("Done printing full expression List");          

        // SyntaxTree = expressionList;
        return expressionList;
    }

}