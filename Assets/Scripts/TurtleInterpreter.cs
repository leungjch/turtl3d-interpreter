using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpreter
{

    public List<Token> TokensList = new List<Token>(); 
    public List<AbstractSyntaxTreeNode> SyntaxTree = new List<AbstractSyntaxTreeNode>(); 


    // Eval
    GameObject Cube;



    // FOR LEXER
    // Enums of possible types of tokens
    public enum TokenType 
    {
        PRIMITIVE,
        IDENTIFIER, // e.g. the "fd" in fd 10, the "x" in x=1. Can be function or variable. 
        CONSTANT,   // e.g. the "10" in fd 10, the "1" in x=1. Just numbers.
        L_PAREN,     // [
        R_PAREN,     // ]
        REPEAT,     // repeat keyword
        EOF         // Always at the end of input

    }

    // FOR LEXER
    // Generic token class
    public class Token 
    {
        public TokenType _tokenType;
        public string _literal; 

        public Token(TokenType tokType, string lit)
        {
            // Had to use underscore because tokenType was already taken
            _tokenType = tokType;
            _literal = lit;
        }
    }


    // FOR LEXER

    // Primitives defined in the logo language
    public enum PrimitiveType 
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    }
    // Dictionary of primitive types aka (reserved) keywords
    public Dictionary<string, PrimitiveType> KeywordLookupTable = new Dictionary<string, PrimitiveType>
    {
        { "fd", PrimitiveType.FORWARD },
        { "bk", PrimitiveType.BACKWARD }
    };


    
    public List<Token> lexer(string text) 
    {
        // Start with an empty list of tokens
        List<Token> tokensList = new List<Token>();

        // Loop through each character in text, handle by "word" 
        int i = 0;
        while (i < text.Length)
        {
            // Build up a literal
            String tokenLiteral = "";
            Char currentChar = text[i];
            // Check the first character.
            // If the first character is alphabetic, then it's a word.
            // Loop until end of word.
            if (Char.IsLetter(text[i]))
            {   
                while (i < text.Length && Char.IsLetter(text[i]))
                {
                    tokenLiteral += text[i];
                    i += 1;
                }

                // TODO
                // Add keyword word checking here
                // For now, assume all words are primitives (fd / bk)
                switch (tokenLiteral)
                {
                    case "repeat":
                        tokensList.Add(new Token(TokenType.REPEAT, tokenLiteral));
                        break;
                    // Else, it's a primitive
                    default:
                        tokensList.Add(new Token(TokenType.PRIMITIVE, tokenLiteral));
                        break;

                }
                
            }

            // If first character is numeric, then it's a number.
            // Loop until end of number. 
            else if (Char.IsDigit(text[i]))
            {
                while (i < text.Length && Char.IsDigit(text[i]))
                {
                    tokenLiteral += text[i];
                    i += 1;
                }
                
                tokensList.Add(new Token(TokenType.CONSTANT, tokenLiteral));
            }

            // Handle parens ("[ ]")
            else if (text[i] == '[')
            {
                tokensList.Add(new Token(TokenType.L_PAREN, "["));
                i+= 1;
            }
            else if (text[i] == ']')
            {
                tokensList.Add(new Token(TokenType.R_PAREN, "]"));
                i += 1;
            }


            else 
            {
                i += 1;
            }
        }

        // Add EOF token at end
        tokensList.Add(new Token(TokenType.EOF, "EOF"));

        // Debugging
        string debugLog = "";
        foreach (Token tok in tokensList)
        {
            debugLog += tok._literal + "  , ";
        }
        Debug.Log(debugLog);
        return tokensList;
    }

    // The parser 
    // Takes TokenList and builds an Abstract Syntax Tree

    // Bracket matching algorithm
    public List<Token> getInner(List<Token> tokensInner, ref int i)
    {
        int depth = 1;
        List<Token> inner = new List<Token>();

        while (depth > 0 && i < tokensInner.Count)
        {
            if (tokensInner[i]._tokenType == TokenType.L_PAREN)
            {
                depth += 1;
            }
            else if (tokensInner[i]._tokenType == TokenType.R_PAREN)
            {
                depth -= 1;
                if (depth == 0)
                {
                    return inner;
                }
            }

            if (i < tokensInner.Count)
            {
                inner.Add(tokensInner[i]);
                i+=1;
            }
            else
            {
                break;
            }

        }
        return inner;
    }

    public List<AbstractSyntaxTreeNode> parser(List<Token> tokens) 
    {
        TokensList = tokens;
        List<AbstractSyntaxTreeNode> expressionList = new List<AbstractSyntaxTreeNode>();
        int i = 0;

        while (i < TokensList.Count) 
        {   
            Token currentToken = TokensList[i]; 

            Debug.Log("Current token: " + currentToken._literal);

            switch (currentToken._tokenType) 
            {
                case TokenType.PRIMITIVE:
                    // Fetch next token (numeric argument, e.g. "20" in fd 20)
                    if (i+1 < TokensList.Count && TokensList[i+1]._tokenType != TokenType.EOF)
                    {
                        i += 1;
                        Token nextToken = TokensList[i];
                        expressionList.Add(
                            new FunctionArgNode(currentToken._literal, Int32.Parse(nextToken._literal))
                        );
                    }
                    break;
                
                case TokenType.REPEAT:
                    // Fetch repeat count (repcount)
                    // Handle bracket matching
                    if (i+1 < TokensList.Count && TokensList[i+1]._tokenType != TokenType.EOF)
                    {
                        i += 1; // repcount
                        int repCount = Int32.Parse(TokensList[i]._literal);
                        // Debug.Log(repCount);
                        i += 1; // (
                        // // if == '['
                        if (i < TokensList.Count && TokensList[i]._tokenType == TokenType.L_PAREN )
                        {
                            i+=1; // first token inside parens
                            List<Token> inner = getInner(TokensList, ref i);
                            TurtleInterpreter interpretInner = new TurtleInterpreter();
                            List<AbstractSyntaxTreeNode> innerExp = interpretInner.parser(inner);
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

                    }
                    break;
                case TokenType.EOF:
                    break;

                default:
                    Debug.Log("Error parsing token");          
                    break;
            }
            i+=1;
        }
        Debug.Log("Printing full expression List");          
        // Debugging
        // foreach (AbstractSyntaxTreeNode nd in expressionList)
        // {
        //     Debug.Log(JsonUtility.ToJson(nd, true));
        //     if (nd.type == AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.REPEAT)
        //     {
        //         Debug.Log("hiya");

        //         RepeatNode rnd = (RepeatNode) nd;
        //         foreach (AbstractSyntaxTreeNode rinner in rnd.inner)
        //         {
        //             Debug.Log("hi");

        //             Debug.Log(JsonUtility.ToJson(rinner, true));
        //         }
        //     }
        // }
        // SyntaxTree = expressionList;
        return expressionList;
    }

    public void eval(List<AbstractSyntaxTreeNode> exprList, bool isRepeat = false) 
    {
        SyntaxTree = exprList;
        // Debug.Log("Debugging");
        // foreach (AbstractSyntaxTreeNode nd in SyntaxTree)
        // {
        //     Debug.Log(JsonUtility.ToJson(nd, true));
        // }

        Turtle turtleScript = GameObject.Find("Cube").GetComponent<Turtle>();
        turtleScript.goHome();
        

        // if we are repeating, don't clear the queue
        if (!isRepeat)
        {
            turtleScript.clearQueue();
        }

        int i = 0;
        while (i < SyntaxTree.Count)
        {
            AbstractSyntaxTreeNode currentNode = SyntaxTree[i];
            switch (currentNode.type)
            {

                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.REPEAT:

                    RepeatNode repeatNode = (RepeatNode) currentNode; 

                    for (int REPCOUNT = 0; REPCOUNT < repeatNode.repeatCount; REPCOUNT++)
                    {
                        eval(repeatNode.inner, true);
                    }
                    break;

                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.FUNCTION_ARG:

                    FunctionArgNode funcNode = (FunctionArgNode) currentNode;

                    switch (funcNode.name) 
                    {
                        case "fd":
                            turtleScript.goForward(funcNode.arguments);
                            break;
                        case "bk":
                            turtleScript.goBackward(funcNode.arguments);
                            break;
                        case "lt":
                            turtleScript.turnLeft(funcNode.arguments);
                            break;
                        case "rt":
                            turtleScript.turnRight(funcNode.arguments);
                            break;
                        case "up":
                            turtleScript.turnUp(funcNode.arguments);
                            break;
                        case "dn":
                            turtleScript.turnDown(funcNode.arguments);
                            break;
                        default:
                            Debug.Log("Error parsing function arg call node"); 
                            break;
                    }
                    break;
                
                default:
                    Debug.Log("Error parsing node"); 
                    break;
            }

            i+=1;
            
        }
    }
}
