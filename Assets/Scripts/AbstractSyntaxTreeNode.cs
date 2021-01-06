using System.Collections;
using System.Collections.Generic;

public class AbstractSyntaxTreeNode
{
    public string name; 
    public AbstractSyntaxTreeNodeType type; 


    public enum AbstractSyntaxTreeNodeType 
    {
        EXPRESSION,
        FUNCTION_ARG,
        FUNCTION_NO_ARG
    } 


    // Class for a node describing a function call with arguments
    // E.g. "fd 10"
    // 
    // public class OperatorNode 
    // {

    // }
    // public class ExpressionNode 
    // {
    //     public AbstractSyntaxTreeNode left;
    //     public AbstractSyntaxTreeNode right;
    //     public OperatorNode operator; 
    // }

}

public class FunctionArgNode : AbstractSyntaxTreeNode
{
    // Attributes unique to functionArg
    
    // Arguments
    public int arguments;
    public FunctionArgNode(string mName, int mValue)
    {
        type = AbstractSyntaxTreeNodeType.FUNCTION_ARG;
        // Function name
        name = mName; 
        // Set arguments
        arguments = mValue;
    }
}