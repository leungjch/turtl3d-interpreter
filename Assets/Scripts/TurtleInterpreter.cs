using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpreter
{

        public void parse(string text) {
            Debug.Log("HELLO");
            string[] tokens = text.Split(' ');


            for (int i = 0; i < tokens.Length; i++) {
                Debug.Log(tokens[i]);
            }

        }

}
