using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 rotation;
    private Vector3 previousPos;
    public Transform target;

    public Camera cam;
    
    public float rotationSpeed = 2F;
    public float zoomSpeed = 200F;

    public float rotX = 0f;
    public float rotY = 0f;

    private float scrollDisplacement = 100f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        // cam.transform.LookAt(target);

        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        // cam.transform.LookAt(target, Vector3.left);

    float distanceCameraTarget = Vector3.Distance(cam.transform.position,target.position);
    // Zoom in and out with scroll wheel
    if (Input.GetAxis("Mouse ScrollWheel") != 0 && distanceCameraTarget >= 5) {
    // cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, target.localPosition, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
    
    scrollDisplacement -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

    } else if (distanceCameraTarget <= 5)  // if too close, move back
    {
        // cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, target.localPosition,  -1);
    scrollDisplacement = 5;
    }


    // Right click drag rotation

    if (Input.GetMouseButtonDown(0))
    {
        previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
    }

    if(Input.GetMouseButton(0))
    {
        Vector3 direction = previousPos - cam.ScreenToViewportPoint(Input.mousePosition);
        cam.transform.Rotate(new Vector3(1, 0 ,0),direction.y * 180);
        cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World );

        previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
    } 

        cam.transform.position = target.position;

        cam.transform.Translate(new Vector3(0, 0, -scrollDisplacement));


    }
}
