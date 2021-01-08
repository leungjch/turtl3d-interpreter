using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer
{
    public string Text; 

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

    public void advanceIndex()
    {
        index += 1;
    }
    public char currentChar()
    {
        return Text[index]; 
    }
    public char peekNextChar()
    {
        if (index+1 < Text.Count)
        {
            return Text[index+1];
        }
    }

    public List<Token> runLexer(string text) 
    {
        // Set text in class
        Text = text; 
        index = 0;



        // Start with an empty list of tokens
        List<Token> tokensList = new List<Token>();

        // Loop through each character in Text, handle by "word" 
        int index = 0;
        while (index < Text.Length)
        {
            // Build up a literal
            String tokenLiteral = "";
            Char currentChar = Text[index];
            // Check the first character.
            // If the first character is alphabetic, then it's a word.
            // Loop until end of word.
            if (Char.IsLetter(Text[index]))
            {   
                while (index < Text.Length && Char.IsLetter(Text[index]))
                {
                    tokenLiteral += Text[index];
                    index += 1;
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
            else if (Char.IsDigit(Text[index]))
            {
                while (index < Text.Length && Char.IsDigit(Text[index]))
                {
                    tokenLiteral += Text[index];
                    index += 1;
                }
                
                tokensList.Add(new Token(TokenType.CONSTANT, tokenLiteral));
            }

            // Handle parens ("[ ]")
            else if (Text[index] == '[')
            {
                tokensList.Add(new Token(TokenType.L_PAREN, "["));
                index+= 1;
            }
            else if (Text[index] == ']')
            {
                tokensList.Add(new Token(TokenType.R_PAREN, "]"));
                index += 1;
            }

            else 
            {
                index += 1;
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
        // Debug.Log(debugLog);
        return tokensList;
    }


}