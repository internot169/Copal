using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Shootable
{
    GameObject player;

    NavMeshAgent agent;

    public float Speed = 0.5f;

    public float YOffset = 180.0f;

    public EnemyGun enemyGun;

    bool noticedPlayer = true;

    [Header("Navigation")]
    [SerializeField] LayerMask groundLayer;
    LayerMask playerLayer;

    Vector3 destPoint;
    bool walkpointSet = false;
    int pauseiteration = 0;
    float scale = 2.5f;
    float nextMove;
    [SerializeField] float range;
    // Start is called before the first frame update

    [Header("AugmentModifications")]
    [Header("Slow")]
    public int slow_tier;
    public int slow_timer;
    [Header("Burn")]
    public int burn_tier;
    public int burn_timer;
    float apply_timer;
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        agent.speed = Speed;
        agent.acceleration = 2000f;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 vectToPlayer = player.transform.position - transform.position;

        // two situations: Noticed player and not
        //if (noticedPlayer){

        // if player is noticed and is in range

        if (!enemyGun.isShooting){
            // ALWAYS be looking at the player
            transform.LookAt( player.transform );
            transform.eulerAngles = new Vector3(0, YOffset + transform.eulerAngles.y,0);

            // we want a location along the path of AI to Player, but with modified location
            // First, reverse vectToPlayer for vector to enemy
            Vector3 VectToEnemy = -vectToPlayer;
            // this neg vector will have magnitude range. Norm and remultiply it
            VectToEnemy.Normalize();
            VectToEnemy = VectToEnemy * enemyGun.range;
            // and now, let's move the AI to player plus that vector
            agent.SetDestination(player.transform.position + VectToEnemy);
        }
        
        
        // player is in range
        if (vectToPlayer.magnitude < enemyGun.range + 1.0f){
            // this tells the gun to shoot
            RaycastHit hit;
            Vector3 GunToPlayer = player.transform.position - enemyGun.gunEnd.position;
            if (enemyGun != null && Physics.Raycast(enemyGun.gunEnd.position, GunToPlayer.normalized, out hit, enemyGun.range+1.0f)){
                enemyGun.isInRange = true;
                enemyGun.shot = hit;
                enemyGun.positionOfHit = enemyGun.gunEnd.position;
                enemyGun.directionOfHit = GunToPlayer.normalized;
            }
        } else{
            if (enemyGun != null){
                // this tells the gun to not shoot
                enemyGun.isInRange = false;
            }
        }

        //}
        //  else{
        //     // we are going to have the ai randomly roam to random locations
        //     // first, check if it is currently roaming.

        //     if (walkpointSet){
        //         agent.SetDestination(destPoint);
        //     } else if (Time.time > nextMove){
        //         destPoint = GenerateRandomPoint();
        //         walkpointSet = true;
        //     }

        //     // keep moving until the distance gets relatively close. 
        //     // first, tell the agent to stop moving for a couple of iterations
        //     // then, we unset walkpoint and wait until next iteration to 
        //     // set the next walkpoint
        //     if (Vector3.Distance(transform.position, destPoint) < 2 && Time.time > nextMove){
        //         walkpointSet = false;
        //         // tell it to stop for a while
        //         nextMove = Time.time + 2.0f;
        //         Debug.Log(nextMove);
        //     }
            
        // }

        if (currentHealth == 0){
            Object.Destroy(this.gameObject);
        }
    }

    // void SearchForDest()
    // {
    //     // pick spot to roam
    //     float Z = Random.Range(-range, range);
    //     float X = Random.Range(-range, range);

    //     destPoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

    //     if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
    //     {
    //         walkpointSet = true;
    //     }
    // }
    
    Vector3 GenerateRandomPoint(){
        // select a random degree
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        NavMeshHit NavHit;
        if(NavMesh.SamplePosition(direction, out NavHit, 100 , NavMesh.AllAreas))
        {
            direction = NavHit.position;
        }
        return direction * scale;
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
            
    }
}
