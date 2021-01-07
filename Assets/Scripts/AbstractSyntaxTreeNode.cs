using System.Collections;
using System.Collections.Generic;

public class AbstractSyntaxTreeNode
{
    public string name; 
    public AbstractSyntaxTreeNodeType type; 


    public enum AbstractSyntaxTreeNodeType 
    {
        PROGRAM_ROOT,
        EXPRESSION,
        FUNCTION_ARG,
        FUNCTION_NO_ARG,
        REPEAT
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

// Root node of the program
public class ProgramNode : AbstractSyntaxTreeNode
{
    public List<AbstractSyntaxTreeNode> code;
    public ProgramNode()
    {
        name = "program";
        type = AbstractSyntaxTreeNodeType.PROGRAM_ROOT;
        code = new List<AbstractSyntaxTreeNode>();
    }
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

public class RepeatNode : AbstractSyntaxTreeNode
{
    public int repeatCount;
    public AbstractSyntaxTreeNode inner;

    public RepeatNode(int m_repeatCount, AbstractSyntaxTreeNode m_inner)
    {
        repeatCount = m_repeatCount;
        inner = m_inner;
    }
}

