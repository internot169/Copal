using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //Speed of the platform
    public float speed = 3.0f;
    //The end position
    public Transform endPos;
    //The start position
    public Transform startPos;

    void Update()
    {
        //How far the object move each update
        var step = speed * Time.deltaTime;
        //Makes the platform move towards the end pos
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, step);

        //if it is close enough, it swaps the start and end pos and goes back to where it came from
        if (Vector3.Distance(transform.position, endPos.position) < 0.001f)
        {
            Transform holdData = startPos;
            startPos = endPos;
            endPos = holdData;
        }
    }
}