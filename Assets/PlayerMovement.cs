using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool movingForward = false;
    bool movingSide = false;

    int forwardForce = 0;
    int sidewaysForce = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Change velocity, not acceleration
        // Add a force once pressed

        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);

        if (w && (!movingForward && !s)){
            rb.AddForce(transform.forward * force * Time.deltaTime);
            forwardForce = 1;
            movingForward = true;
        } else if (s && (!movingForward && !w)){
            rb.AddForce(-transform.forward * force * Time.deltaTime);
            forwardForce = -1;
            movingForward = true;
        } else {
            // remove force
            rb.AddForce(-forwardForce * transform.forward * force * Time.deltaTime);
            forwardForce = 0;
            movingForward = false;
        }

        if (a && (!movingSide && !d)){
            rb.AddForce(-transform.right * force * Time.deltaTime);
            sidewaysForce = -1;
            movingSide = true;
        } else if (d && (!movingSide && !a)){
            rb.AddForce(transform.right * force * Time.deltaTime);
            sidewaysForce = 1;
            movingSide = true;
        } else {
            // remove force
            rb.AddForce(-sidewaysForce * transform.right * force * Time.deltaTime);
            sidewaysForce = 0;
            movingSide = false;
        }
    }
}
