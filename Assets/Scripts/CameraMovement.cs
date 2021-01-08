using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 rotation;
    public Transform target;
    public float rotationSpeed = 2F;
    public float zoomSpeed = 80F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target);

        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        transform.LookAt(target, Vector3.left);

    float distanceCameraTarget = Vector3.Distance(transform.localPosition,target.localPosition);
    // Zoom in and out with scroll wheel
    if (Input.GetAxis("Mouse ScrollWheel") != 0 && distanceCameraTarget >= 5) {
    transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
    } else if (distanceCameraTarget <= 5)  // if too close, move back
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition,  -1);
    }


    // Right click drag rotation
    if(Input.GetMouseButton(0))
    {
     transform.RotateAround(target.transform.position, 
                                     transform.up,
                                     -Input.GetAxis("Mouse X")*rotationSpeed);

     transform.RotateAround(target.transform.position, 
                                     transform.right,
                                     -Input.GetAxis("Mouse Y")*rotationSpeed);
    } 

    }
}
