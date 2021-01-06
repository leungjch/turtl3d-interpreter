using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpreter
{
    // Enums of possible types of tokens
    public enum TokenType 
    {
        Number,
        Word,
        LParen,
        RParen
    }

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
            if (Char.IsLetter(currentChar))
            {   
                Char restOfWordChar = text[i];
                while (Char.IsLetter(restOfWordChar) && i < text.Length)
                {
                    restOfWordChar = text[i];
                    tokenLiteral += restOfWordChar;
                    i += 1;
                }
                
                tokensList.Add(new Token(TokenType.Word, tokenLiteral));
            }

            // If first character is numeric, then it's a number.
            // Loop until end of number. 
            else if (Char.IsDigit(currentChar))
            {
                Char restOfNumChar = text[i];
                while (Char.IsDigit(restOfNumChar) && i < text.Length)
                {
                    restOfNumChar = text[i];
                    tokenLiteral += restOfNumChar;
                    i += 1;
                }
                
                tokensList.Add(new Token(TokenType.Number, tokenLiteral));
            }
            else 
            {
                i += 1;
            }

        }

        foreach (Token tok in tokensList)
        {
            Debug.Log(tok._literal);
        }
        return tokensList;


    }

    // The parser 
    public void parser(string[] tokens) 
    {

    }
}
