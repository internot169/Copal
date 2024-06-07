using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Wumpus : Shootable
{   
    // Rigidbody to apply knocback forces. 
    public Rigidbody playerRB;

    // assigned in editor. 
    public float KnockBackForce;

    // public reference to player, is assigned in script. 
    public GameObject Player;

    // deprecated, but held here just in case it's necessary in the future. 
    UnityEngine.AI.NavMeshAgent agent;

    // this checks the area of the stomp to apply damage later. 
    // both of these are public just in case. 
    public GameObject StompArea;

    // reference to the warning area for the stomp. 
    public GameObject StompWarning;

    // health of the wumpus. 
    public int wumpusHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        // rewrite the health of the shootable box with the wumpus health. 
        // allows in script modification.
        this.currentHealth = wumpusHealth;
        // find teh player, and allow the player to be hit more often. 
        Player = GameObject.Find("Player");
        Player.GetComponent<PlayerInfo>().TimeSinceHurtLimit = 0f;
        // find and hide the stopm warning. 
        StompWarning = GameObject.Find("StompWarn");
        StompWarning.SetActive(false);
        // get the area of the stomp to update the stomp detection. 
        StompArea = GameObject.Find("StompArea");
    }

    // take damage for the wumpus. 
    public override void Damage(int damageAmount){
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            //if health has fallen below zero, deactivate it 
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    // helper to calculate and apply the force. 
    void ApplyForceUp(){
        // the actual directoin between the player and its location. 
        Vector3 PureDirection = Player.transform.position - transform.position;
        // modify the pure direction, but upwards, to ensure that the player doesn't get clipped into the bottom. 
        Vector3 KBDirection = new Vector3(PureDirection.x, 5f, PureDirection.z);
        // add force to the player to effect knockback. 
        playerRB.AddForce(KBDirection * KnockBackForce * Time.deltaTime);
    }

    // method to apply the stomp attack. 
    public IEnumerator Stomp(){
        // show
        Debug.Log("Charging");
        // show the warning
        StompWarning.SetActive(true);
        // wait for player to dodge. 
        yield return new WaitForSeconds(5);
        // hide the warning. 
        StompWarning.SetActive(false);
        Debug.Log("Stomping");
        // if the stomp area says the player is inside, then the player takes 20 damage and gets knocked back. 
        if (StompArea.GetComponent<Stomp>().playerIn == true){
            Player.GetComponent<PlayerInfo>().TakeDamage(20);
            ApplyForceUp();
        }
    }

    
}