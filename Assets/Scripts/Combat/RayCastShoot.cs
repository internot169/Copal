using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class RayCastShoot : MonoBehaviour
{
    public int gunDamage = 1;
    public int altFireDamage = 2;
    public float fireRate = 0.25f;
    public float altFireRate = 2f;
    public float altFireAOE = 5f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public GameObject prefab;

    [SerializeField]
    private TrailRenderer BulletTrail;

    private Camera fpsCam;
    private float shotDuration = 0.01f;
    private AudioSource gunAudio;
    private float nextFire;
    private float nextAltFire;
    
    // increment as needed. 
    private int main_modify = 0;

    private int alt_modify = 0;

    private bool has_drone = false;

    // main fire. 
    private bool has_main_freeze;
    private bool has_main_dot;

    private bool has_main_vamp;

    // alt fire
    private bool has_alt_freeze;
    private bool has_alt_dot;

    private bool has_alt_vamp;
    
    [Header("Augments")]
    private GameObject drone;

    private GameObject player;

    private int droneDamageMain;

    private int droneDamageAlt;

    void Start()
    {
        // laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        // should honestly make it a public field but im lazy. 
        drone = GameObject.Find("Drone");
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            // laserLine.enabled = true;
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            // laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                TrailRenderer trail = Instantiate(BulletTrail, gunEnd.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                // call drone trail
                if (has_drone){
                    StartCoroutine(drone.GetComponent<DroneCode>().SpawnDroneTrail(BulletTrail, hit.point, hit.normal));  
                }
                if (has_main_freeze){
                    hit.collider.gameObject.GetComponent<Enemy>().MarkSlows();
                }
                if (has_main_dot){
                    hit.collider.gameObject.GetComponent<Enemy>().MarkBurns();
                }

                if(has_main_vamp){
                    player.GetComponent<PlayerInfo>().Heal(10f);
                }

                // weird interaction with how they're organized in editor, requires this for enemy and wumpus
                Shootable health = hit.collider.transform.GetComponent<Shootable>();
                if (health == null && hit.collider.transform.parent != null){
                    health = hit.collider.transform.parent.GetComponent<Shootable>();
                }
                if (health != null)
                {
                    health.Damage(gunDamage + main_modify + droneDamageMain);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, gunEnd.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, gunEnd.position + transform.forward.normalized, Vector3.zero, false));
                // call drone trail
                StartCoroutine(drone.GetComponent<DroneCode>().SpawnDroneTrail(BulletTrail, hit.point, Vector3.zero));  
            }
            
        }
        if (Input.GetButtonDown("AltFire") && Time.time > nextAltFire)
        {
            nextAltFire = Time.time + altFireRate;

            if (has_alt_vamp){
                player.GetComponent<PlayerInfo>().TakeDamage(10);
            }

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange)){
                // If something is hit. stores information in the variable hit from RaycastHit

                // Different from above, we instantiate a explosion at the location
                // OverlapSphere returns all colliders within the area
                // center is hit.point
                // radius is predetermined
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, altFireAOE);
                Instantiate(prefab, hit.point, Quaternion.identity);

                foreach (var hitCollider in hitColliders)
                {
                    Debug.Log(hitCollider);
                    // check if is player
                    PlayerInfo health = hitCollider.transform.GetComponent<PlayerInfo>();
                    if (health == null && hit.collider.transform.parent != null){
                        health = hit.collider.transform.parent.GetComponent<PlayerInfo>();
                    }
                    if (health != null){
                        health.TakeDamage(altFireDamage + alt_modify);
                    }
                    else{
                        Shootable bhealth = hitCollider.transform.GetComponent<Shootable>();
                        if (bhealth == null && hit.collider.transform.parent != null){
                            bhealth = hit.collider.transform.parent.GetComponent<Shootable>();
                        }
                        if (bhealth != null)
                        {
                            bhealth.Damage(altFireDamage + droneDamageAlt);
                            if (has_alt_freeze){
                                hitCollider.gameObject.GetComponent<Enemy>().MarkSlows();
                            }
                            if (has_alt_dot){
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                            }
                            if(has_alt_dot){
                                // didn't have foresight to make it mark off function, so just gonna have to do it this way. 
                                // could change, but would require too many sweepgin changes to be worth it imo. 
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                                hitCollider.gameObject.GetComponent<Enemy>().MarkBurns();
                            }
                        }

                    }

                    if (hitCollider.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        Vector3 closePoint = hitCollider.ClosestPoint(hit.point);
                        Vector3 forceVector = closePoint - hit.point;
                        hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(forceVector * hitForce * 10f);
                    }
                }
            }
        }
    }
    
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not

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

    // augment flippers

    public void ChangeSlowMain(bool state){
        has_main_freeze = state;
    }

    public void ChangeDOTMain(bool state){
        has_main_dot = state;
    }
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
    }

    public void ChangeDroneMain(bool state){
        droneDamageMain = state ? 5: 0;
    }

    public void ChangeDroneAlt(bool state){
        droneDamageAlt = state ? 3 : 0;
    }
}
