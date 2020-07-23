//Reference script:https://www.youtube.com/watch?v=LbDQHv9z-F0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{ 
    
    public Transform Target; // the target which camera following
    public float CameraMoveSpeed = 120.0f;
    public float inputSensitivity = 1.0f;
    public float clamAngle = 80.0f;
    public float mouseX;
    public float mouseY;
    public float Direction;
  
    // Use this for initialization
    void Start()
    {       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
      
    void LateUpdate()
    {  
        CameraUpdater();
    }
    void CameraUpdater()
    {   
        mouseX += Input.GetAxis("Mouse X") * inputSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * inputSensitivity;
        mouseY = Mathf.Clamp(mouseY, -clamAngle, clamAngle);
        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0); //rotate the camera
       
        //move towards the game object that is the target
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
    }
    public float getMouseYdirection()
    {
        return mouseY;
    }
}
