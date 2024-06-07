using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Shootable
{
    // indicates player
    GameObject player;

    // indicates current navmeshagent on enemy prefab
    NavMeshAgent agent;

    // indicates normal speed of agent
    public float Speed = 0.5f;

    // indicates the turn amount to offset the original mesh
    public float YOffset = 180.0f;

    // indicates enemy script
    public EnemyGun enemyGun;

    // layers of navigation
    [Header("Navigation")]
    [SerializeField] LayerMask groundLayer;
    LayerMask playerLayer;

    // items to indicate augment impacts on enemy
    [Header("AugmentModifications")]
    [Header("Slow")]
    // amount and timer of slow
    public int slow_tier;
    public int slow_timer;
    [Header("Burn")]
    // amount and timer of burn
    public int burn_tier;
    public int burn_timer;
    
    // current enemy rigidbody
    Rigidbody m_Rigidbody;

    void Start()
    {
        // grab the components to use alter and set speed/accel
        m_Rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        agent.speed = Speed;
        agent.acceleration = 2000f;
    }

    // Update is called once per frame
    void Update()
    {   
        // grabs vector to player
        Vector3 vectToPlayer = player.transform.position - transform.position;

        // if not currently shooting
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
            // AKA that vector away from player
            agent.SetDestination(player.transform.position + VectToEnemy);
        }
        
        
        // player is in range
        if (vectToPlayer.magnitude < enemyGun.range + 1.0f){
            // this tells the gun to shoot
            // raycast hit to location, use in phys.raycast
            RaycastHit hit;
            // vector of gunpoint to position
            Vector3 GunToPlayer = player.transform.position - enemyGun.gunEnd.position;
            // if has gun + can hit player
            if (enemyGun != null && Physics.Raycast(enemyGun.gunEnd.position, GunToPlayer.normalized, out hit, enemyGun.range+1.0f)){
                // tell gun its in range
                enemyGun.isInRange = true;
                // identify the shot taken
                enemyGun.shot = hit;
                // identify position of shot origin
                enemyGun.positionOfHit = enemyGun.gunEnd.position;
                // identify the direction the shot is going
                enemyGun.directionOfHit = GunToPlayer.normalized;
            }
        } else{ // otherwise (not in range)
            if (enemyGun != null){
                // this tells the gun to not shoot
                enemyGun.isInRange = false;
            }
        }

        // if current health is zero
        if (currentHealth == 0){
            // delete it
            Object.Destroy(this.gameObject);
        }
    }

    // when slowed
    public void MarkSlows()
    {
        // add/reset timer
        slow_timer = 10;
        // if not maxed tier add a tier
        if(slow_tier < 10)
        {
            slow_tier += 1;
        }
    }

    public void MarkBurns()
    {
        // add/reset timer
        burn_timer = 10;
        // if not maxed tier add a tier
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
