using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // 
    public Transform player;
    
    // Sensitivity for mouse
    public float sensitivityX = 100F;
    public float sensitivityY = 100F;

    // clamping rotation
    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Update()
    {
        // Rotate y by mouse movement from left to right
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        
        // Rotate x by mouse movement from up to down
        transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        player.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime, 0);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }
}
