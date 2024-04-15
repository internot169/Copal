using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Shootable
{
    GameObject player;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {   
        // if I have a dest, set the destination
        if (walkpointSet)
        {
            agent.SetDestination(destPoint);
        }else{
            SearchForDest();
        }

        // if i'm close to dest, reset destination. 
        if(Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        // pick spot to roam
        float Z = Random.Range(-range, range);
        float X = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }
}
