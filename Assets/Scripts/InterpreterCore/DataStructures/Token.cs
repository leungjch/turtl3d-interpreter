    // Generic token class
    public class Token 
    {

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
            REPCOUNT,   // repcount keyword
            TO,         // function declaration (start)
            END,        // function declaration (end)
            ADD,        // addition
            SUB,        // subtraction
            MUL,        // multiplication
            DIV,        // division
            EOF         // Always at the end of input

        }

        public TokenType _tokenType;
        public string _literal; 

        public Token(TokenType tokType, string lit)
        {
            // Had to use underscore because tokenType was already taken
            _tokenType = tokType;
            _literal = lit;
        }
    }