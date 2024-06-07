using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WumpusAi : MonoBehaviour
{
    // all nonpublic values below are assigned on startup. 
    // hold a reference to player. 
    GameObject Player;
    
    // this is for targeting the charge. 
    // This object allows us to modify the location and destination of the boss. 
    UnityEngine.AI.NavMeshAgent agent;

    // these are gameobjects which refer to warnings. 
    // this is to show the warnings before an attack is applied. 
    GameObject Charge_Warn;

    // warning for sky beam attack. 
    GameObject Sky_Warn;

    // this is a sky beam that the wumpus can spawn in. 
    // this is assigned on startup. 
    GameObject Sky_Beam;

    // this raycast hit is used for the drone barrage attack. 
    public RaycastHit hit;

    // this is a reference to an object which holds within it all the drones for the drone barrage. 
    public GameObject DronesParent;

    // a boolean for whether or not the wumpus is using the charging attack. 
    bool isCharging = false;

    // these are default values, likely different in editor and in build. 
    // these values are used for the agent, to determine the maximum speed and the rate of acceleration. 
    public float Speed = 75.0f;
    public float Accel = 2000.0f;

    // this holds a reference to the wumpus object, which is the parent of this. 
    Wumpus wumpus;

    /* 
    A short description of the hierarchy:
    There is a parent gameobject, called Wumpus. 
    The large wumpus script handles the taking damage of wumpus
    This script handles the AI and movement of the wumpus*as well as its warnings and drones)
    It also handles a few attacks that warrants it being in the parent. 
    */
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = Speed;
        agent.acceleration = Accel;

        Player = GameObject.Find("Player");
        Charge_Warn = GameObject.Find("ChargeWarn");
        Charge_Warn.SetActive(false);
        Sky_Beam = GameObject.Find("SkyBeam");
        Sky_Beam.SetActive(false);
        Sky_Warn = GameObject.Find("SkyBeamWarn");
        Sky_Warn.SetActive(false);
        wumpus = GetComponentInChildren<Wumpus>();

        StartCoroutine(Pick_Action());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCharging)
        {
            // wumpus doesn't random roam, cause it notices player upon entering

            // if player is noticed and is in range
            // ALWAYS be looking at the player
            // however, never shift z angle as that angles the wumpus weirdly.
            transform.LookAt( Player.transform );
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

            agent.SetDestination(Player.transform.position);

            // if distance to player is less than 8.5, wumpus is JUST TOUCHING player
            Vector3 vectToPlayer = Player.transform.position - transform.position;
            // so stop it immediately by having it "reach its destination" immediately
            if (vectToPlayer.magnitude < 8.5f){
                agent.SetDestination( transform.position );
            }
        }
    }

    // this randomly picks the next attack to be used. 
    // this gets called again at the end of every attack. 
    IEnumerator Pick_Action(){
        // roll a dice
        float pick = UnityEngine.Random.Range(0f, 1f);
        // pick the attacks. 
        if(pick < 0.25f){
            StartCoroutine(DroneBarrage());
        }else if(pick < 0.50f){
            StartCoroutine(Stomp());
        }else if(pick < 0.75f){
            StartCoroutine(Charge());
        }else if (pick < 1f){
            StartCoroutine(SkyBeam());
        }
        // no difference between yield break and yield return null. 
        // it doesn't run anything the next frame anyways, so no need to instantly stop. 
        yield return null;
    }

    // this is hard coded for the charge attack. 
    IEnumerator Charge(){
        // first notes that the wumpus is charging, which will freeze the movement. 
        isCharging = true;
        // target the player .
        transform.LookAt(Player.transform.position);
        // calulate the distance such that it can be given over to the charge warning
        Vector3 distance =  Player.transform.position - transform.position;
        // this tells the warning to scale its size accordingly. 
        Charge_Warn.GetComponent<ChargeWarnScale>().Scale(distance.magnitude);
        // show the warning. 
        Charge_Warn.SetActive(true);
        // set the destination, but freeze the wumpus by causing its speed to be 0. 
        agent.SetDestination(Player.transform.position);
        agent.speed = 0;
        // wait 5 seconds. 
        // this allows the player to dodge. 
        yield return new WaitForSeconds(5);
        // hide the warning
        Charge_Warn.SetActive(false);
        // give it a lot of acceleration and speed so it can charge at the destnation. 
        agent.acceleration = 5000f;
        agent.speed = 120f;
        // if the wumpus hasn't reached the destination, then stay trapped in the loop. 
        while(agent.remainingDistance != 0){
            yield return null;
        }
        // then, reset the values to the normal. 
        Debug.Log("reset");
        agent.speed = Speed;
        agent.acceleration = Accel;

        // now that the charge is finished, mark it in the boolean. 
        isCharging = false;
        // pick the next action. 
        StartCoroutine(Pick_Action());
    }

    IEnumerator Stomp(){
        // call to the child wumpus to execute the stomp. 
        yield return StartCoroutine(wumpus.Stomp());
        // yield return awaits for the action to finish, so after it's finished then pick next action. 
        StartCoroutine(Pick_Action());
    }

    // drone hit barrage attack. 
    IEnumerator DroneBarrage(){
        // shoot 5 times
        for (int times = 0; times < 5; times++)
        {   
            // for every drone in the drone group
            foreach (Transform DroneChild in DronesParent.transform){
                // calculate a different vector from them to the player, then tell them to shoot. 
                WumpusDroneGun Script = DroneChild.GetComponent<WumpusDroneGun>();
                Vector3 vectToPlayer = Player.transform.position - DroneChild.GetChild(0).transform.position;
                if (Physics.Raycast(Script.gunEnd.position, vectToPlayer, out hit, Mathf.Infinity)){
                    // hand over the values such that they can do the checks just like enemy. 
                    Script.positionOfHit = Script.gunEnd.position;
                    Script.directionOfHit = vectToPlayer;
                    DroneChild.GetComponent<WumpusDroneGun>().Shoot(hit);
                }
            }
            // wait 0.5 seconds between each shot. 
            // since the drone shots are asynchronus, it will line up fine. 
            yield return new WaitForSeconds(0.5f);
        }
        // after the barrage is over, pick the next action. 
        StartCoroutine(Pick_Action());
    }

    // sky beam attack. 
    IEnumerator SkyBeam(){
        // move the sky warn to be centered on the player, then show it. 
        Sky_Warn.transform.position = new Vector3(Player.transform.position.x, 1, Player.transform.position.z);
        Sky_Warn.SetActive(true);
        // wait 5 seconds to allow the player to dodge. 
        yield return new WaitForSeconds(5);
        // hide the warning, and activate the beam. 
        Sky_Warn.SetActive(false);
        Sky_Beam.SetActive(true);
        Sky_Beam.transform.position = new Vector3(Sky_Warn.transform.position.x, Sky_Warn.transform.position.y + 20, Sky_Warn.transform.position.z);
        // teleport the beam to where the warning is, which is it firing the sky beam. 
        // hold the beam there for 2 seconds. 
        yield return new WaitForSeconds(2);
        // deactivate the beam
        Sky_Beam.SetActive(false);
        // pick the next action. 
        StartCoroutine(Pick_Action());
    }
}
