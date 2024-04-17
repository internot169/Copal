using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCode : MonoBehaviour
{
    public IEnumerator SpawnDroneTrail(TrailRenderer BulletTrail, Vector3 HitPoint, Vector3 HitNormal)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        TrailRenderer Trail = Instantiate(BulletTrail, transform.position, Quaternion.identity);
        Debug.Log("Drone Drawn");
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
