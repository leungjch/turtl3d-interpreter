using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    float scale;
    float progress = 0.0f;
    int commandsPerUpdate = 100;

    public Queue<Command> commandQueue = new Queue<Command>();

    // Keep track of position history for drawing line
    public List<Vector3> positionHistory = new List<Vector3>();

    // Line renderer object
    LineRenderer line; 

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
        line = gameObject.AddComponent<LineRenderer>();
        //Set the speed of the GameObject
        scale = 1; // value between 0 and 100
    }

    void Update()
    {
        // Debug.Log("Num commands" + commandQueue.Count);
        // Get current command
        for (int i = 0; i < commandsPerUpdate; i++)
        {
            if (commandQueue.Count > 0)
            {
                Command currentCommand = commandQueue.Peek();
                float valueIncrement = currentCommand.val;
                // Debug.Log("Current command" + currentCommand.action + progress);

                Vector3 oldPos = m_Rigidbody.position;

                switch (currentCommand.action)
                {
                    case "fd":
                        // m_Rigidbody.position += transform.forward * valueIncrement * scale * Time.deltaTime;
                        m_Rigidbody.position += transform.forward * valueIncrement * scale;

                        break;
                    case "bk":
                        m_Rigidbody.position -= transform.forward * valueIncrement * scale;
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

                Vector3 newPos = m_Rigidbody.position;

                positionHistory.Add(newPos); 
                line.SetVertexCount(positionHistory.Count);
                line.SetPosition(positionHistory.Count-1, newPos);
                commandQueue.Dequeue();

                // if (progress >= currentCommand.val)
                // {
                //     commandQueue.Dequeue();
                //     progress = 0.0f;
                // }
                // else 
                // {
                //     progress += scale * Time.deltaTime;
                // }
            }
        }


        // Keyboard controls (for testing)
        // // Go forwards
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position += transform.forward * scale * Time.deltaTime;
        }

        // Go backwards
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position -= transform.forward * scale * Time.deltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * scale, Space.World);
        }

        // Rotate Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * scale, Space.World);
        }

        // Rotate Up
        if (Input.GetKey(KeyCode.Q))
        {
            goHome();

            //Rotate the sprite about the X axis in the positive direction
            transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * scale, Space.World);
        }

        // Rotate Down
        if (Input.GetKey(KeyCode.W))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * scale, Space.World);
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
        // Rest command queue and turtle position
        goHome();
        commandQueue.Clear();

        // Clear line history
        positionHistory = new List<Vector3>();
    }

}
