using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    bool start = true;

    void Awake()
    {
        transform.position = startPos.position;
    }

    void Update()
    {
        /*float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        Vector3 interpolatedPosition = Vector3.Lerp(transform.position, endPos.position, interpolationRatio);

        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);*/

        if (start = true){
            while (transform != endPos)
            {
                Vector3.MoveTowards(transform.position, endPos.position, 1f);
            }
        }
    }
}