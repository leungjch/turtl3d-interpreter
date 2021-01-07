using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditor : MonoBehaviour
{
    public GameObject inputGameObject;
    public string userText;
    public InputField field;

    TurtleInterpreter interpreter = new TurtleInterpreter();
    // public void Start() {
    //     iField = iFieldGameObject.GetComponent<InputField>();
    // }
    // Update is called once per frame
    public void GetUserText()
    {
        Debug.Log(field.text);
        userText = field.text;
        // Debug.Log("DONE");

        List<TurtleInterpreter.Token> tokenList = interpreter.lexer(userText);
        List<AbstractSyntaxTreeNode> syntaxTree = interpreter.parser(tokenList);
        interpreter.eval(syntaxTree);
    }
}
