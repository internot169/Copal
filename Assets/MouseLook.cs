using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public float minX = -70f;
    public float maxX = 70f;
    public float lookSpeed = 3f;

    private Vector2 rotation = Vector2.zero;
    public void Update()
    {
        /*
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0,rotation.y) * lookSpeed;
        Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);

        transform.Rotate(0f, 0f, 0f);
        */
    }
}
