using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpreter
{

    public string Text; 
    public List<Token> TokensList = new List<Token>(); 
    public List<AbstractSyntaxTreeNode> SyntaxTree = new List<AbstractSyntaxTreeNode>(); 

    Lexer lexer;
    Parser parser;
    Eval eval;

    public TurtleInterpreter(string text)
    {
        Text = text; 
        lexer = new Lexer();
        parser = new Parser();
        eval = new Eval();
    }

    public void runInterpreter()
    {
        TokensList = lexer.runLexer(Text);
        SyntaxTree = parser.runParser(TokensList); 
        eval.runEval(SyntaxTree); 
    }

}
