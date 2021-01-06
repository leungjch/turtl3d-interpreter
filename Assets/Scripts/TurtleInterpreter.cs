using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpreter
{

    public List<Token> TokensList = new List<Token>(); 

    // FOR LEXER
    // Enums of possible types of tokens
    public enum TokenType 
    {
        Number,
        Word,
        LParen,
        RParen
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

    // FOR PARSER
    public enum ExpressionType 
    {
        MovementExpression
    }


    // Abstract Syntax Tree structure
    public class ExpressionTreeNode
    {
        public ExpressionType _expressionType;
        public int _repeat;
        public List<ExpressionTreeNode> _subexpressions;
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

        // Debugging
        foreach (Token tok in tokensList)
        {
            Debug.Log(tok._literal);
        }
        TokensList = tokensList; 
        return tokensList;
    }

    // The parser 
    // Takes TokenList and builds an Abstract Syntax Tree
    public void parser() 
    {
        expressionList = new List<ExpressionTreeNode>();
        int i = 0;
        while (i < TokensList.Count) 
        {   
            Token currentToken = TokensList[i]; 
            switch (currentToken) 
            {
                case TokenType.Word:
                    expressionList.Add(new ExpressionTreeNode(currentToken._tokenType, ))

        
                case TokenType.Number:    

                default:
                    Debug.Log("Error parsing token");          
            }
        }
    }
}
