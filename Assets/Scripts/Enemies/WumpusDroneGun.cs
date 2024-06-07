using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WumpusDroneGun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int gunDamage = 5;
    public float reloadTime = 1.0f;
    public float range = 5.0f;
    
    [Header("General Gun Information")]
    public Transform gunEnd;
    public TrailRenderer BulletTrail;
    private float nextFire;

    [Header("Shooting information")]
    public bool isInRange = false;
    public GameObject player;
    public RaycastHit shot;

    public Vector3 directionOfHit;
    public Vector3 positionOfHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    Assumes you already handed me a raycast hit that connects a enemy to player
    I will also assume that you have LOS from this

    Major potential bug: 

    If raycast hit does not update its values over time, then a major bug will 
    be the fact that it will do a phantom hit even if you move out of the way. 

    If this is the case, there are 2 options, both of which do the same thing. 

    1) Calculate another raycast using the previous hit point of the other raycast. 
    This basically tries to check if the player is still there. However, this has
    issues with if the player moves out of the distance of the previous hit point, 
    so do this carefully. 

    2) Give the original parameters for the raycast that you determined the raycast with, 
    extract those, and pass into the shoot function and propagate down to do the recalculation 
    at the end. 

    TLDR: you might need to do a recalculation on the raycast depending on how it works. 
    */
    public void Shoot(RaycastHit hit)
    {
        // this is where it shoots from, you should change if you want diff logic. 
        Vector3 rayOrigin = transform.position;
        
        // generate trail
        TrailRenderer trail = Instantiate(BulletTrail, transform.position, Quaternion.identity);

        // start coroutine to warn player w/trail
        StartCoroutine(WarnPlayer(trail, hit.point, hit.normal, true, hit));
    }

    // This just shows a trail to warn the player, and then will
    // shoot the bullet. 
    private IEnumerator WarnPlayer(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, RaycastHit hit)
    {
        // Essentially the same logic as the player gun.
        // First, determine the start location of the shot. 
        Vector3 startPosition = Trail.transform.position;
        // calculate the distance from the enemy to the location of the shot. 
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        // remaining distance for later. 
        float remainingDistance = distance;

        // while the trail has not crossed the destination, 
        while (remainingDistance > 0)
        {   
            // move it closer based on the remaining distance and the distance. 
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            // update the remaining distance
            remainingDistance -= 200f * Time.deltaTime;

            // pause and move closer the next frame. 
            yield return null;
        }
        // then make sure that it's exactly at the hit point. 
        // the loop can (and probably will due to floating point) take you past the position of the hit. 
        // thus, move it back to ensure that it's correct. 
        Trail.transform.position = HitPoint;

        // then, wait for a bit so the player has time to move. 
        // this is the same time as the trail lasts for. 
        yield return new WaitForSeconds(2);

        // after the trail disappears, recalculate the hit. 
        StartCoroutine(DealDamage(hit));
    }

    // deals damage after giving warning. 
    private IEnumerator DealDamage(RaycastHit hit)
    {
        // create a new raycasthit, check. 
        RaycastHit check;
        // then, populate check with another raycast hit that checks if the player is still within range of the shot. 
        Physics.Raycast(positionOfHit, directionOfHit, out check, Mathf.Infinity);
        // this if statement is to ensure that the gameobject that check finds and the player are the same
        if (GameObject.ReferenceEquals(check.collider.gameObject, player)){
            // if so, then make the player take damage. 
            player.GetComponent<PlayerInfo>().TakeDamage(gunDamage);
        // due to weird interactions, one should check the parent as well. 
        // since objects (like the drone) can also be hit, should check them as well and use them to process hits. 
        } else if (GameObject.ReferenceEquals(check.collider.gameObject.transform.parent.gameObject, player)){
            // once again, make the player take damage if the player is hit. 
            player.GetComponent<PlayerInfo>().TakeDamage(gunDamage);
        }
        
        // end the script. 
        // it's necessary to use this to end on this exact frame. 
        // yield return null would last for 1 more frame, and this script should end as soon as the shot is complete. 
        yield break;
    }
}
