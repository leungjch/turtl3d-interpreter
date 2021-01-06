using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditor : MonoBehaviour
{
    public string userText;
    public GameObject inputField;

    TurtleInterpreter interpreter = new TurtleInterpreter();
    // public void Start() {

    //     userText = inputField.GetComponent<Text>().text;
    //     interpreter.lexer(userText);


    // }
    // Update is called once per frame
    public void GetUserText()
    {
        userText = inputField.GetComponent<Text>().text;
        interpreter.lexer(userText);
    }
}
