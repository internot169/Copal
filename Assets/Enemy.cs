using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Shootable
{
    GameObject player;

    NavMeshAgent agent;

    [Header("Navigation")]
    [SerializeField] LayerMask groundLayer;
    LayerMask playerLayer;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    // Start is called before the first frame update

    [Header("AugmentModifications")]
    [Header("Slow")]
    int slow_tier;
    int slow_timer;
    [Header("Burn")]
    int burn_tier;
    int burn_timer;
    float apply_timer;
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
        // if it's been an eight second since last tick, then
        // apply again. 
        if (apply_timer > 0.125)
        {
            ApplyEffects();
            apply_timer = 0;
        }
        apply_timer += Time.deltaTime;
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

    public void MarkSlows()
    {
        slow_timer = 10;
        if(slow_tier < 10)
        {
            slow_tier += 1;
        }
    }

    public void MarkBurns()
    {
        burn_timer = 10;
        if(burn_tier < 10)
        {
            burn_tier += 1;
        }
    }

    public void ApplyEffects()
    {
        // Apply slows
        agent.speed = 10 - slow_tier;
        if (slow_timer == 0){
            // busy writing, theoretically we should add a case
            // where we do nothing if both are 0, but i'm lazy. 
            slow_tier = 0;
        }else{
            // to be honest, it doesn't matter if the slow
            // timer ticks to negative, but its better to be
            // safe in case of overflow. 
            slow_timer -= 1;
        }
        Debug.Log("slow");
        Debug.Log(slow_timer + " " + slow_tier);
        // Apply burn
        if (burn_timer % 2 == 0){
            Damage(burn_tier/2);
        }
        if (burn_timer == 0){
            // busy writing, theoretically we should add a case
            // where we do nothing if both are 0, but i'm lazy. 
            burn_tier = 0;
        }else{
            // to be honest, it doesn't matter if the slow
            // timer ticks to negative, but its better to be
            // safe in case of overflow. 
            burn_timer -= 1;
        }
        Debug.Log("burn");
        Debug.Log(burn_timer + " " + burn_tier);
            
    }
}
