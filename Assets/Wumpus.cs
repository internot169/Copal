using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class Wumpus : MonoBehaviour
{   
    public Rigidbody rb;
    public float KnockBackForce;

    GameObject Player;

    UnityEngine.AI.NavMeshAgent agent;

    public GameObject StompArea;

    public GameObject StompWarning;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        StompWarning = GameObject.Find("StompWarn");
        StompWarning.SetActive(false);
        StompArea = GameObject.Find("StompArea");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnCollisionStay(Collision collision){
        Collider collider = collision.collider;
        GameObject player = collider.gameObject;
        Debug.Log(player.name);
        ApplyForce();
        // do damage based on roaming, charging, and stomp
        Player.GetComponent<PlayerInfo>().TakeDamage(20);

    }

    void ApplyForce(){
        Vector3 direction = Player.transform.position - transform.position;
        rb.AddForce(direction * KnockBackForce * Time.deltaTime);
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
            ApplyForce();
        }
    }

    
}
