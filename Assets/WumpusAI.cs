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

    Wumpus wumpus;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Player = GameObject.Find("Player");
        Charge_Warn = GameObject.Find("ChargeWarn");
        Charge_Warn.SetActive(false);
        wumpus = GetComponentInChildren<Wumpus>();

        StartCoroutine(Charge());
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    IEnumerator Stomp(){
        yield return StartCoroutine(wumpus.Stomp());
    }
}
