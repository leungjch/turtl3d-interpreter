using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    float m_Speed;
    float progress = 0.0f;

    public Queue<Command> commandQueue = new Queue<Command>();

    public class Command 
    {
        public string action;
        public int val; 
        public Command (string newAction, int newVal)
        {
            val = newVal;
            action = newAction;
        }
    }

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
        //Set the speed of the GameObject
        m_Speed = 80; // value between 0 and 100
    }

    void Update()
    {
        // Debug.Log("Num commands" + commandQueue.Count);
        // Get current command
        if (commandQueue.Count > 0)
        {
            Command currentCommand = commandQueue.Peek();
            float valueIncrement = currentCommand.val / 100.0f;
            Debug.Log("Current command" + currentCommand.action + currentCommand.val);

            switch (currentCommand.action)
            {
                case "fd":
                    m_Rigidbody.position += transform.forward * valueIncrement * m_Speed;
                    break;
                case "bk":
                    m_Rigidbody.position -= transform.forward * valueIncrement * m_Speed;
                    break;
                case "lt": 
                    transform.Rotate(new Vector3(1, 0, 0) * currentCommand.val);
                    progress = 100;
                    break;
                case "rt":
                    transform.Rotate(new Vector3(1, 0, 0) * currentCommand.val);
                    progress = 100;
                    break;  
                case "up":
                    transform.Rotate(new Vector3(0, 1, 0) * currentCommand.val);
                    progress = 100;
                    break;
                case "dn":
                    transform.Rotate(new Vector3(0, 1, 0) * -currentCommand.val);
                    progress = 100;
                    break;
            }

            if (progress >= 100.0f)
            {
                commandQueue.Dequeue();
                progress = 0.0f;
            }
            else 
            {
                progress += m_Speed;
            }
        }

        // Keyboard controls (for testing)
        // // Go forwards
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position += transform.forward * m_Speed * Time.deltaTime;
        }

        // Go backwards
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position -= transform.forward * m_Speed * Time.deltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * m_Speed, Space.World);
        }

        // Rotate Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * m_Speed, Space.World);
        }

        // Rotate Up
        if (Input.GetKey(KeyCode.Q))
        {
            goHome();

            //Rotate the sprite about the X axis in the positive direction
            transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * m_Speed, Space.World);
        }

        // Rotate Down
        if (Input.GetKey(KeyCode.W))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * m_Speed, Space.World);
        }


        

    }

    public void goForward(int val)
    {
        // m_Rigidbody.position += transform.forward * val;
        Command newCommand = new Command("fd", val);
        commandQueue.Enqueue(newCommand);
    }


    public void goBackward(int val)
    {
        Command newCommand = new Command("bk", val);
        commandQueue.Enqueue(newCommand);
    }

    public void turnLeft(int deg)
    {
        Command newCommand = new Command("lt", deg);
        commandQueue.Enqueue(newCommand);
    }

    public void turnRight(int deg)
    {
        Command newCommand = new Command("rt", deg);
        commandQueue.Enqueue(newCommand);
    }

    public void turnUp(int deg)
    {
        // transform.Rotate(new Vector3(1, 0, 0) * deg);
        Command newCommand = new Command("up", deg);
        commandQueue.Enqueue(newCommand);
    }

    public void turnDown(int deg)
    {
        Command newCommand = new Command("dn", deg);
        commandQueue.Enqueue(newCommand);
    }

    public void goHome()
    {
        m_Rigidbody.transform.position = new Vector3(0, 0, 0);
        m_Rigidbody.transform.rotation = Quaternion.Euler(0, 0, 0);

    }
    public void clearQueue()
    {
        goHome();
        commandQueue.Clear();
    }

}
