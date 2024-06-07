using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class RayCastShoot : MonoBehaviour
{
    // constants for the gun. These will be modified in editor. 
    // These are declared default values. Actual values in build may differ. 
    public int gunDamage = 1;
    public int altFireDamage = 2;
    public float fireRate = 0.25f;
    public float altFireRate = 2f;
    public float altFireAOE = 5f;
    public float weaponRange = 50f;
    public float hitForce = 100f;

    // these values are not attached. they will be assigned values in editor. 
    // Treat these as if they are already assigned upon startup. 
    public Transform gunEnd;
    public GameObject prefab;

    [SerializeField]
    
    // The trail for the shot.
    // Acts as a muzzle flash/ visual cue that you've shot. 
    private TrailRenderer BulletTrail;

    // private reference to camera. Gets assigned later. 
    private Camera fpsCam;
    
    // constant for how long the trail should last. 
    private float shotDuration = 0.01f;

    // audio source for the gun to play audio.
    private AudioSource gunAudio;

    // timers for handling the cooldown period for shooting. 
    private float nextFire;
    private float nextAltFire;
    
    // increment as needed. 
    // these are values that the augments system utilizes to modify gun damage. 
    private int main_modify = 0;

    private int alt_modify = 0;

    // this is to ensure that an augment works. 
    // if true, this hands the targeting data to a drone so another trail can be drawn. 
    private bool has_drone = false;

    // main fire. 
    // these values indicate the state of augments surrounding the main attack. 
    private bool has_main_freeze;
    private bool has_main_dot;
    private bool has_main_vamp;

    // alt fire
    // these booleans handle the state of augments surrounding the alternate attack. 
    private bool has_alt_freeze;
    private bool has_alt_dot;
    private bool has_alt_vamp;
    
    [Header("Augments")]
    // these are references to other objects required for the code to run. 
    // They are assigned in startup. 
    private GameObject drone;

    private GameObject player;

    // this value is to modify damage for the drone augments specifically. 
    // This value works differently from the other damage increase to
    // help balance this augment. 
    private int droneDamageMain;

    private int droneDamageAlt;

    void Start()
    {
        // This will assign the gun audio being played, as well as the camera. 
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        // this finds references to the drone and player for later use. 
        drone = GameObject.Find("Drone");
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // Upon left click is clicked, and the gun can fire again
        if (Input.GetButtonDown("Fire1") &&  nextFire > fireRate)
        {
            // first calculate the next time that one can fire the gun. 
            nextFire = 0;
            // Then calculate a vector 3 in the center of the fps camera. 
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // allocate this name to be filled by Physics.Raycast. 
            RaycastHit hit;

            // if a ray cast from the direction of the fps camera in a specific range intersects a game object, 
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // create a new trail that starts fro mthe end of the gun. 
                TrailRenderer trail = Instantiate(BulletTrail, gunEnd.position, Quaternion.identity);
                
                // start a script that asynchronusly draws the trail. 
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                // call drone trail
                if (has_drone){
                    // tell the drone to also draw the trail from itself if the player has the drone augment. 
                    // The drone trail is cosmetic, the damage is increased directly for the gun. 
                    StartCoroutine(drone.GetComponent<DroneCode>().SpawnDroneTrail(BulletTrail, hit.point, hit.normal));  
                }

                // if the player has the freeze augment for the main attack, 
                if (has_main_freeze){
                    // tell the gameobject to apply the slows to itself. 
                    hit.collider.gameObject.GetComponent<Enemy>().MarkSlows();
                }

                // similar logic for the player having the dot augment. 
                if (has_main_dot){
                    hit.collider.gameObject.GetComponent<Enemy>().MarkBurns();
                }

                // if the player has the vampirism augment, 
                if(has_main_vamp){
                    // then heal the player for some hp. 
                    // PlayerInfo holds the logic for handling the cases. 
                    player.GetComponent<PlayerInfo>().Heal(10f);
                }

                // weird interaction with how they're organized in editor, requires this for enemy and wumpus
                // This checks if the gameobject intersected has a script that inherits from shootable. 
                // shootable is attached to any object that can be shot. 
                Shootable health = hit.collider.transform.GetComponent<Shootable>();
                if (health == null && hit.collider.transform.parent != null){
                    // due to weird organization for wumpus, if the first object is null, then check the parent 
                    // essentially an edge case specifically to handle the wumpus. 
                    health = hit.collider.transform.parent.GetComponent<Shootable>();
                }

                // given that the object is shootable, 
                if (health != null)
                {
                    // then deal damage based on the augments. 
                    health.Damage(gunDamage + main_modify + droneDamageMain);
                }
                if (hit.rigidbody != null)
                {
                    // given that the object has a rigid body, give it some knockback. 
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                // otherwise, give a random trail that just shoots forward. 
                // this edge case handles when you don't hit anything in particular. 
                TrailRenderer trail = Instantiate(BulletTrail, gunEnd.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, gunEnd.position + transform.forward.normalized, Vector3.zero, false));
                // call drone trail
                StartCoroutine(drone.GetComponent<DroneCode>().SpawnDroneTrail(BulletTrail, hit.point, Vector3.zero));  
            }
            
        }
        // if the player uses the alternate fire, and it is ready to fire, 
        if (Input.GetButtonDown("AltFire") && altFireRate < nextAltFire)
        {
            // reset the timer as it jsut fired. 
            nextAltFire = 0;

            // if the character has the vampirism modifier for the alternate fire, 
            if (has_alt_vamp){
                // deal extra damage to the player in accordance with the behavior.
                // take damage handles the death of the player.  
                player.GetComponent<PlayerInfo>().TakeDamage(10);
            }

            // once again, calculate the value of the center of the camera. 
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // create allocation ahaed of time to be filled by Physics.Raycast.
            RaycastHit hit;
            // calculate a raycast from the fps camera to the point of interest. Give this value to hit. 
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange)){
                // If something is hit. stores information in the variable hit from RaycastHit

                // Different from above, we instantiate a explosion at the location
                // OverlapSphere returns all colliders within a spherical area. 
                // center is hit.point
                // radius is predetermined
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, altFireAOE);
                // draw the explosion. 
                Instantiate(prefab, hit.point, Quaternion.identity);

                // check each collider in the AOE. 
                foreach (var hitCollider in hitColliders)
                {
                    // check if is player
                    // Only the player has the playerInfo, this is to do self damaging as the 
                    // alt fire can damage yourself. 
                    PlayerInfo health = hitCollider.transform.GetComponent<PlayerInfo>();
                    // same check as above just in case of weird heirachy organization. 
                    if (health == null && hit.collider.transform.parent != null){
                        health = hit.collider.transform.parent.GetComponent<PlayerInfo>();
                    }
                    // given that the player is in the range, then make the player take damage. 
                    if (health != null){
                        health.TakeDamage(altFireDamage + alt_modify);
                    }
                    // otherwise, this is likely not the player. as such, process very similar to before. 
                    else{
                        // see if we can access the shootable component to deal damage. 
                        Shootable bhealth = hitCollider.transform.GetComponent<Shootable>();
                        // again, check in the parent just in case of weird hierarchy organization. 
                        if (bhealth == null && hit.collider.transform.parent != null){
                            bhealth = hit.collider.transform.parent.GetComponent<Shootable>();
                        }
                        // given that there is a shootable object in the collider, then 
                        if (bhealth != null)
                        {
                            // deal the damage, and also the modified damage just in cse. 
                            bhealth.Damage(altFireDamage + droneDamageAlt + alt_modify);

                            // if the alternate fire has the freeze augment, then apply slows. 
                            if (has_alt_freeze){
                                hitCollider.gameObject.GetComponent<Enemy>().MarkSlows();
                            }
                            
                            // if the alternate fire has the DOT augment, then apply burns. 
                            if(has_alt_dot){
                                // didn't have the foresight to allow the method to mark the level of stacks, so
                                // will have to use this to apply the requisite stacks. 
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                            }
                        }

                    }

                    // apply the knockback that the alt fire has. 

                    if (hitCollider.gameObject.GetComponent<Rigidbody>() != null)
                    {   
                        // calculate the point that is closest, then calculate a vector for the force to go along
                        Vector3 closePoint = hitCollider.ClosestPoint(hit.point);
                        Vector3 forceVector = closePoint - hit.point;
                        // apply the force to the rigidbody. 
                        hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(forceVector * hitForce * 10f);
                    }
                }
            }
        }
        // add the amount of time elapsed in this frame, such that the cooldown timer will work. 
        nextAltFire += Time.deltaTime;
        nextFire += Time.deltaTime;
    }
    
    // an asynchronus function to spawn the trail/muzzle flash for the gun. 
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // mark the start position of the trail, 
        Vector3 startPosition = Trail.transform.position;
        // calculate the distance between the trail's current position(which should be the player), and the point that it flies to. 
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        // save this as a value. 
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            // move the trail closer to the final value
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            // then reduce the remaining distance. 
            // the 200f here is a form of speed. 
            remainingDistance -= 200f * Time.deltaTime;

            // for this function, yield return null allows it to pause running for this frame, and then start up again the next frame. 
            yield return null;
        }

        // just to make sure, the trail ends at the hit point. 
        // this is because the trail can actually fly past the hit point. This constrains it. 
        Trail.transform.position = HitPoint;

        // destroy the trail. 
        Destroy(Trail.gameObject, Trail.time);
    }

    // augment flippers

    // fairly basic flippers that allow you to change the state of the augments. 
    public void ChangeSlowMain(bool state){
        has_main_freeze = state;
        // all augments will add some damage. 
        // this statement will change the value of main modify depending on if the augment gets activated or deactivated. 
        // if it's deactivated, then change it to 0. 
        // for balance, only some augments can modify damage, so it's a case by case basis. 
        main_modify = state ? 2: 0;
    }

    public void ChangeDOTMain(bool state){
        has_main_dot = state;
        // same as above. 
        main_modify = state ? 2: 0;
    }
    
    // these two alternate fires are meant more to spread the effect effectively. 
    public void ChangeSlowAlt(bool state){
        has_alt_freeze = state;
    }
    public void ChangeDOTAlt(bool state){
        has_alt_dot = state;
    }

    public void ChangeVampMain(bool state){
        has_main_vamp = state;
    }

    public void ChangeVampAlt(bool state){
        has_alt_vamp = state;
        // vamp alternate fire is high risk, high reward. 
        alt_modify = state ? 10: 0;
    }

    public void ChangeDroneMain(bool state){
        droneDamageMain = state ? 5: 0;
    }

    public void ChangeDroneAlt(bool state){
        droneDamageAlt = state ? 3 : 0;
    }
}
