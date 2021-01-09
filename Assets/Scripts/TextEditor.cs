using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditor : MonoBehaviour
{
    public GameObject inputGameObject;
    public string userText;
    public InputField field;

    // public void Start() {
    //     iField = iFieldGameObject.GetComponent<InputField>();
    // }
    // Update is called once per frame
    public void GetUserText()
    {
        Debug.Log(field.text);
        userText = field.text;

        TurtleInterpreter interpreter = new TurtleInterpreter(userText);
        interpreter.runInterpreter();
    }
}
