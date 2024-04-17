using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCode : MonoBehaviour
{
    public IEnumerator SpawnDroneTrail(TrailRenderer Trail, Vector3 HitPosition, Vector3 HitNormal)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 HitPoint = HitPosition - transform.position;
        HitPoint.Normalize();
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= 200f * Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = HitPoint;

        Destroy(Trail.gameObject, Trail.time);
    }
}
