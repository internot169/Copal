using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WumpusAi : MonoBehaviour
{

    GameObject Player;
    
    UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update

    GameObject Charge_Warn;

    GameObject Sky_Warn;

    GameObject Sky_Beam;

    public RaycastHit hit;

    public GameObject DronesParent;

    bool isCharging = false;

    public float Speed = 75.0f;
    public float Accel = 200.0f;

    Wumpus wumpus;
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
            Vector3 vectToPlayer = Player.transform.position - transform.position;

            // wumpus doesn't random roam, cause it notices player upon entering

            // if player is noticed and is in range
            // ALWAYS be looking at the player
            // however, never shift z angle because that makes stuff cringe
            transform.LookAt( Player.transform );
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

            agent.SetDestination(transform.position + vectToPlayer);
        }
    }

    IEnumerator Pick_Action(){
        float pick = UnityEngine.Random.Range(0f, 1f);
        if(pick < 0.25f){
            StartCoroutine(DroneBarrage());
        }else if(pick < 0.50f){
            StartCoroutine(Stomp());
        }else if(pick < 0.75f){
            StartCoroutine(Charge());
        }else if (pick < 1f){
            StartCoroutine(SkyBeam());
        }
        yield return null;
    }

    IEnumerator Charge(){
        isCharging = true;
        transform.LookAt(Player.transform.position);
        Charge_Warn.SetActive(true);
        agent.SetDestination(Player.transform.position);
        agent.speed = 0;
        yield return new WaitForSeconds(5);
        Charge_Warn.SetActive(false);
        agent.acceleration = 5000f; // we want it to be speedy and stop immediately
        agent.speed = 120f;
        while(agent.remainingDistance != 0){
            yield return null;
        }
        Debug.Log("reset");
        agent.speed = Speed;
        agent.acceleration = Accel;

        isCharging = false;
        StartCoroutine(Pick_Action());
    }

    IEnumerator Stomp(){
        yield return StartCoroutine(wumpus.Stomp());
        StartCoroutine(Pick_Action());
    }

    IEnumerator DroneBarrage(){
        for (int times = 0; times < 5; times++)
        {
            foreach (Transform DroneChild in DronesParent.transform){
                WumpusDroneGun Script = DroneChild.GetComponent<WumpusDroneGun>();
                Vector3 vectToPlayer = Player.transform.position - DroneChild.GetChild(0).transform.position;
                if (Physics.Raycast(Script.gunEnd.position, vectToPlayer, out hit, Mathf.Infinity)){
                    Script.positionOfHit = Script.gunEnd.position;
                    Script.directionOfHit = vectToPlayer;
                    DroneChild.GetComponent<WumpusDroneGun>().Shoot(hit);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(Pick_Action());
    }

    IEnumerator SkyBeam(){
        Sky_Warn.transform.position = new Vector3(Player.transform.position.x, 1, Player.transform.position.z);
        Sky_Warn.SetActive(true);
        yield return new WaitForSeconds(5);
        Sky_Warn.SetActive(false);
        Sky_Beam.SetActive(true);
        Sky_Beam.transform.position = new Vector3(Sky_Warn.transform.position.x, Sky_Warn.transform.position.y + 20, Sky_Warn.transform.position.z);
        yield return new WaitForSeconds(2);
        Sky_Beam.SetActive(false);
        StartCoroutine(Pick_Action());
    }
}
