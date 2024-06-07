using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [Header("Gun Stats")]
    // these starts can be modified in editor, these are just default values. 
    public int gunDamage = 5;
    public float reloadTime = 1.0f;
    public float range = 4.0f;
    
    [Header("General Gun Information")]
    // these are assigned in editor, treat as if they are assigned on startup or before. 
    public Transform gunEnd;
    public TrailRenderer BulletTrail;
    // private value that acts as a timer. 
    private float nextFire;

    [Header("Shooting information")]
    // these get updated by the enemy script, which will update the state of the gun. 
    public bool isInRange = false;
    public bool isShooting = false;
    // this is public just in case. 
    // it allows to manually assign in editor if we need to. 
    public GameObject player;
    // this holds the data surrounding the shot that's being taken. 
    // the method by which the shooting works will be explained below.
    public RaycastHit shot;

    // this information is necessary to recalculate the hit after drawing in the warning. 
    public Vector3 directionOfHit;
    public Vector3 positionOfHit;

    // Start is called before the first frame update
    void Start()
    {
        // make sure the gun knows where the player is
        // this is imperative for calculations for vectors. 
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // check for the cooldown of the gun. 
        if (reloadTime < nextFire){
            // code safety, make sure that we are not shooting until in range. 
            isShooting = false;

            if (isInRange){
                // now that the enemy is in range, set it to true. 
                isShooting = true;
                // this should also be removed since it's a timer
                nextFire = 0;
                // run the fire script. 
                Shoot(shot);
            }
        }
        nextFire += Time.deltaTime;
    }

    /*
    the shot variable is given by the enemy script, which updates it directly. 

    from this, the enemy first draws a warning "shot", which draws a trail that persists. This tells the player
    to move out of the way, or to warn that the enemy is about to shoot there. 

    Then, the enemy will recalculate a new raycasthit that checks if the player is hit again. 
    if so, then it does damage. Otherwise, the player essentially dodged the hit. 

    This allows us to easily implement hitscan for the enemy without it feeling like cheating or impossible to beat.  
    */
    public void Shoot(RaycastHit hit)
    {
        // As this script is attached to the gun, the transform of the gameobject should be the location
        // from which the shot is taken. 
        Vector3 rayOrigin = transform.position;
        
        // create a new trail object to draw the tracer at the current location. 
        TrailRenderer trail = Instantiate(BulletTrail, transform.position, Quaternion.identity);

        // start the script that warns the player. 
        // this is asynchronus to allow the script to pause during the warning period. 
        StartCoroutine(WarnPlayer(trail, hit.point, hit.normal, true, hit));
    }

    // This script draws a trail that warns the player of an incoming shot. 
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
        yield return new WaitForSeconds(BulletTrail.time);

        // after the trail disappears, recalculate the hit. 
        StartCoroutine(DealDamage());
    }

    // deals damage after giving warning. 
    private IEnumerator DealDamage()
    {
        // create a new raycasthit, check. 
        RaycastHit check;
        // then, populate check with another raycast hit that checks if the player is still within range of the shot. 
        Physics.Raycast(positionOfHit, directionOfHit, out check, range + 1.0f);
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
        
        // after shooting, flip the boolean. 
        isShooting = false;
        
        // end the script. 
        // it's necessary to use this to end on this exact frame. 
        // yield return null would last for 1 more frame, and this script should end as soon as the shot is complete. 
        yield break;
    }
}
