using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 3.0f;
    public Transform endPos;
    public Transform startPos;

    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, step);

        if (Vector3.Distance(transform.position, endPos.position) < 0.001f)
        {
            Transform holdData = startPos;
            startPos = endPos;
            endPos = holdData;
        }
    }
}