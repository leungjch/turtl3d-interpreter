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
        REPEAT,
        REPCOUNT,
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
    public bool isRepcount = false;
    public FunctionArgNode(string mName, int mValue, bool mIsRepCount = false)
    {
        type = AbstractSyntaxTreeNodeType.FUNCTION_ARG;
        // Function name
        name = mName; 
        // Set arguments
        arguments = mValue;
        isRepcount = mIsRepCount;
    }

}


public class RepeatNode : AbstractSyntaxTreeNode
{
    public int repeatCount;
    public List<AbstractSyntaxTreeNode> inner;

    public RepeatNode(int m_repeatCount, List<AbstractSyntaxTreeNode> m_inner)
    {
        name = "repeat";
        type = AbstractSyntaxTreeNodeType.REPEAT;
        repeatCount = m_repeatCount;
        inner = m_inner;
    }
}

public class RepcountNode : AbstractSyntaxTreeNode
{
    public RepcountNode()
    {
        name = "repcount";
        type = AbstractSyntaxTreeNodeType.REPCOUNT;
    }
}
