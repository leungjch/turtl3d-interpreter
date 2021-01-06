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

                
                tokensList.Add(new Token(TokenType.PRIMITIVE, tokenLiteral));
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
            else 
            {
                i += 1;
            }
        }

        // Add EOF token at end
        tokensList.Add(new Token(TokenType.EOF, "EOF"));

        // Debugging
        foreach (Token tok in tokensList)
        {
            Debug.Log(tok._literal + tok._tokenType);
        }
        TokensList = tokensList; 
        return tokensList;
    }

    // The parser 
    // Takes TokenList and builds an Abstract Syntax Tree
    public void parser() 
    {
        List<AbstractSyntaxTreeNode> expressionList = new List<AbstractSyntaxTreeNode>();
        int i = 0;
        while (i < TokensList.Count) 
        {   
            Token currentToken = TokensList[i]; 

            Debug.Log("Current token: " + currentToken._literal);


            switch (currentToken._tokenType) 
            {
                case TokenType.PRIMITIVE:
                    if (i+1 < TokensList.Count)
                    {
                        i += 1;
                        Token nextToken = TokensList[i];
                        expressionList.Add(
                            new FunctionArgNode(currentToken._literal, Int32.Parse(nextToken._literal))
                        );  

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

        // Debugging
        foreach (AbstractSyntaxTreeNode nd in expressionList)
        {
            Debug.Log(JsonUtility.ToJson(nd, true));
            // if (nd is FunctionArgNode fan)
            // {
            //     Debug.Log(fan.arguments);
            // }
        }
        SyntaxTree = expressionList;
    }

    public void eval() 
    {
        Turtle turtleScript = GameObject.Find("Cube").GetComponent<Turtle>();
        turtleScript.goHome(); 


        int i = 0;
        while (i < SyntaxTree.Count)
        {
            AbstractSyntaxTreeNode currentNode = SyntaxTree[i];
            switch (currentNode.type)
            {
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
