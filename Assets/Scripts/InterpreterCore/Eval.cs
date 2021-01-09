using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eval
{

    // Eval
    GameObject Cube;
    Turtle turtleScript;
    int evalIndex;
    List<AbstractSyntaxTreeNode> SyntaxTree = new List<AbstractSyntaxTreeNode>();

    // State
    Dictionary<string, List<AbstractSyntaxTreeNode>> FunctionDefinitionLookup = new Dictionary<string, List<AbstractSyntaxTreeNode>>();
    Dictionary<string, int> VariableLookup = new Dictionary<string, int>(); 

    public Eval()
    {
        turtleScript = GameObject.Find("Cube").GetComponent<Turtle>();
    }
    // Modified eval constructor for passing old states (useful when looping / calling recursive)
    public Eval(Dictionary<string, List<AbstractSyntaxTreeNode>> oldFunctionDefinitionLookup, Dictionary<string, int> oldVariableLookup)
    {
        turtleScript = GameObject.Find("Cube").GetComponent<Turtle>();
        FunctionDefinitionLookup = oldFunctionDefinitionLookup;
        VariableLookup = oldVariableLookup;
    }



    public void advanceIndex()
    {
        evalIndex += 1;
    }

    public AbstractSyntaxTreeNode peekCurrentNode()
    {
        return SyntaxTree[evalIndex];
    }
    public AbstractSyntaxTreeNode peekNextNode()
    {
        // if (evalIndex+1 < SyntaxTree.Count)
        // {
            return SyntaxTree[evalIndex+1];
        // }
    }
    

    public void runEval(List<AbstractSyntaxTreeNode> syntaxTree, bool isRepeat = false, int repCount = 0) 
    {

        evalIndex = 0;
        SyntaxTree = syntaxTree;

        Debug.Log("Debugging syntax tree with index" + isRepeat + " Repcount is" + repCount);
        foreach (AbstractSyntaxTreeNode nd in syntaxTree)
        {
            Debug.Log(JsonUtility.ToJson(nd, true));
        }

        // if we are repeating, don't clear the queue
        if (!isRepeat)
        {
            turtleScript.clearQueue();
        }

        while (evalIndex < syntaxTree.Count)
        {
            AbstractSyntaxTreeNode currentNode = peekCurrentNode();
            switch (currentNode.type)
            {
                // If we encounter a function definition, store the definition in the lookup table
                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.FUNCTION_DEFINITION:

                    FunctionDefinitionNode functionDefinitionNode = (FunctionDefinitionNode) currentNode;
                    
                    string funcName = functionDefinitionNode.name;
                    List<AbstractSyntaxTreeNode> funcDefn = functionDefinitionNode.definition;

                    FunctionDefinitionLookup.Add(funcName, funcDefn);

                    break;

                // If we encounter a repeat statement, repeatedly execute inner expression by REPCOUNT times
                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.REPEAT:

                    RepeatNode repeatNode = (RepeatNode) currentNode; 

                    for (int REPCOUNT = 0; REPCOUNT < repeatNode.repeatCount; REPCOUNT++)
                    {
                        Eval evalInner = new Eval(FunctionDefinitionLookup, VariableLookup);
                        evalInner.runEval(repeatNode.inner, isRepeat = true, repCount = REPCOUNT);
                    }
                    break;

                // If we encounter a function (with 0 arguments) call, execute the function
                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.FUNCTION_NO_ARG:

                    FunctionNoArgNode funcNoArgNode = (FunctionNoArgNode) currentNode;
                    
                    string funcNoArgName = funcNoArgNode.name;
                        Debug.Log("FOUND FUNC" + funcNoArgName);

                    // Check if function name is a user-defined function
                    if (FunctionDefinitionLookup.ContainsKey(funcNoArgName))
                    {
                        Debug.Log("FOUND FUNC" + funcNoArgName);
                        List<AbstractSyntaxTreeNode> funcNoArgDefn = FunctionDefinitionLookup[funcNoArgName];
                        Eval evalFunc = new Eval(FunctionDefinitionLookup, VariableLookup);
                        evalFunc.runEval(funcNoArgDefn, isRepeat = true, repCount);

                    }

                    break;


                // If we encounter a function (with 1 argument) call, execute the function
                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.FUNCTION_ARG:

                    FunctionArgNode funcNode = (FunctionArgNode) currentNode;
                    int args; 
                    if (funcNode.isRepcount)
                    {
                        args = repCount;
                    }
                    else
                    {
                        args = funcNode.arguments;
                    }

                    switch (funcNode.name) 
                    {
                        case "fd":
                            turtleScript.goForward(args);
                            break;
                        case "bk":
                            turtleScript.goBackward(args);
                            break;
                        case "lt":
                            turtleScript.turnLeft(args);
                            break;
                        case "rt":
                            turtleScript.turnRight(args);
                            break;
                        case "up":
                            turtleScript.turnUp(args);
                            break;
                        case "dn":
                            turtleScript.turnDown(args);
                            break;
                        default:
                            Debug.Log("Error parsing function arg call node"); 
                            break;
                    }
                    break;
                
                // Else syntax error
                default:
                    Debug.Log("Error parsing node"); 
                    break;
            }

            advanceIndex();
        }
    }
}