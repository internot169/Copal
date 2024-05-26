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

    Wumpus wumpus;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
    }

    IEnumerator Pick_Action(){
        float pick = UnityEngine.Random.Range(0f, 1f);
        if(pick < 0.33f){
            StartCoroutine(Charge());
        }else if(pick < 0.66f){
            StartCoroutine(Stomp());
        }else if (pick < 1f){
            StartCoroutine(SkyBeam());
        }
        yield return null;
    }

    IEnumerator Charge(){
        transform.LookAt(Player.transform.position);
        Charge_Warn.SetActive(true);
        agent.SetDestination(Player.transform.position);
        agent.speed = 0;
        yield return new WaitForSeconds(5);
        Charge_Warn.SetActive(false);
        agent.acceleration = 100;
        agent.speed = 120;
        while(agent.remainingDistance != 0){
            yield return null;
        }
        agent.acceleration = 8;
        agent.speed = 5;

        StartCoroutine(Pick_Action());
    }

    IEnumerator Stomp(){
        yield return StartCoroutine(wumpus.Stomp());
        StartCoroutine(Pick_Action());
    }

    IEnumerator SkyBeam(){
        Sky_Warn.transform.position = Player.transform.position;
        Sky_Warn.SetActive(true);
        yield return new WaitForSeconds(5);
        Sky_Warn.SetActive(false);
        Sky_Beam.SetActive(true);
        Sky_Beam.transform.position = Sky_Warn.transform.position;
        yield return new WaitForSeconds(2);
        Sky_Beam.SetActive(false);
    }
}
