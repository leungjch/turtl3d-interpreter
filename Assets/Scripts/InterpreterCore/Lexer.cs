using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer
{
    public string Text; 
    public int lexerIndex;


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
        lexerIndex += 1;
    }
    public char peekCurrentChar()
    {
        return Text[lexerIndex];
    }
    public char peekNextChar()
    {
            return Text[lexerIndex+1];
    }

    public List<Token> runLexer(string text) 
    {
        // Set text in class
        Text = text; 
        lexerIndex = 0;

        // Start with an empty list of tokens
        List<Token> tokensList = new List<Token>();

        // Loop through each character in Text, handle by "word" 
        while (lexerIndex < Text.Length)
        {
            // Build up a literal
            String tokenLiteral = "";
            Char currentChar = peekCurrentChar();
            // Check the first character.
            // If the first character is alphabetic, then it's a word.
            // Loop until end of word.
            if (Char.IsLetter(peekCurrentChar()))
            {   
                while (lexerIndex < Text.Length && Char.IsLetter(peekCurrentChar()))
                {
                    tokenLiteral += peekCurrentChar();
                    advanceIndex();
                }

                // TODO
                // Add keyword word checking here
                // For now, assume all words are primitives (fd / bk)
                switch (tokenLiteral)
                {
                    case "repeat":
                        tokensList.Add(new Token(Token.TokenType.REPEAT, tokenLiteral));
                        break;
                    // Else, it's a primitive
                    default:
                        tokensList.Add(new Token(Token.TokenType.PRIMITIVE, tokenLiteral));
                        break;
                }
            }

            // If first character is numeric, then it's a number.
            // Loop until end of number. 
            else if (Char.IsDigit(peekCurrentChar()))
            {
                while (lexerIndex < Text.Length && Char.IsDigit(peekCurrentChar()))
                {
                    tokenLiteral += peekCurrentChar();
                    advanceIndex();
                }
                
                tokensList.Add(new Token(Token.TokenType.CONSTANT, tokenLiteral));
            }

            // Handle parens ("[ ]")
            else if (peekCurrentChar() == '[')
            {
                tokensList.Add(new Token(Token.TokenType.L_PAREN, "["));
                advanceIndex();
            }
            else if (peekCurrentChar() == ']')
            {
                tokensList.Add(new Token(Token.TokenType.R_PAREN, "]"));
                advanceIndex();
            }

            else 
            {
                advanceIndex();
            }
        }

        // Add EOF token at end
        tokensList.Add(new Token(Token.TokenType.EOF, "EOF"));

        // Debugging
        string debugLog = "";
        foreach (Token tok in tokensList)
        {
            debugLog += tok._literal + "  , ";
        }
        Debug.Log(debugLog);
        return tokensList;
    }


}