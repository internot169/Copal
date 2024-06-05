using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class Wumpus : Shootable
{   
    public Rigidbody rb;
    public float KnockBackForce;

    public GameObject Player;

    UnityEngine.AI.NavMeshAgent agent;

    public GameObject StompArea;

    public GameObject StompWarning;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Player.GetComponent<PlayerInfo>().TimeSinceHurtLimit = 0f;
        StompWarning = GameObject.Find("StompWarn");
        StompWarning.SetActive(false);
        StompArea = GameObject.Find("StompArea");
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    void ApplyForceUp(){
        Vector3 PureDirection = Player.transform.position - transform.position;
        Vector3 KBDirection = new Vector3(PureDirection.x, 5f, PureDirection.z);
        rb.AddForce(KBDirection * KnockBackForce * Time.deltaTime);
    }

    public IEnumerator Stomp(){
        // show
        Debug.Log("Charging");
        StompWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        StompWarning.SetActive(false);
        Debug.Log("Stomping");
        if (StompArea.GetComponent<Stomp>().playerIn == true){
            Player.GetComponent<PlayerInfo>().TakeDamage(20);
            ApplyForceUp();
        }
    }

    
}