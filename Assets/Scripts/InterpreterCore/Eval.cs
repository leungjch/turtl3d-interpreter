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



    public Eval()
    {
        turtleScript = GameObject.Find("Cube").GetComponent<Turtle>();
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

        // Debug.Log("Debugging syntax tree with index" + isRepeat);
        // foreach (AbstractSyntaxTreeNode nd in syntaxTree)
        // {
        //     Debug.Log(JsonUtility.ToJson(nd, true));
        // }

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
                case AbstractSyntaxTreeNode.AbstractSyntaxTreeNodeType.REPEAT:

                    RepeatNode repeatNode = (RepeatNode) currentNode; 

                    for (int REPCOUNT = 0; REPCOUNT < repeatNode.repeatCount; REPCOUNT++)
                    {
                        Eval evalInner = new Eval();
                        evalInner.runEval(repeatNode.inner, isRepeat = true, repCount = REPCOUNT);
                    }
                    break;

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
                
                default:
                    Debug.Log("Error parsing node"); 
                    break;
            }

            advanceIndex();
        }
    }
}