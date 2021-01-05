using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    float m_Speed;

    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
        //Set the speed of the GameObject
        m_Speed = 10.0f;
    }

    void Update()
    {
        // Rotate Down
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position += transform.forward * m_Speed * Time.deltaTime;
        }

        // Rotate Up
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.position -= transform.forward * m_Speed * Time.deltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * m_Speed, Space.World);
        }


        // Rotate Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * m_Speed, Space.World);
        }


        // Rotate Up
        if (Input.GetKey(KeyCode.Q))
        {
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

}
