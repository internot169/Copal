using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpusCollisionScript : MonoBehaviour
{
    Wumpus wumpus;

    // Start is called before the first frame update
    void Start()
    {
        wumpus = transform.parent.gameObject.GetComponent<Wumpus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // on each collision
    void OnCollisionStay(Collision collision){
        // grab the collider it touched
        Collider collider = collision.collider;
        // hypothetically this is the player
        GameObject player = collider.gameObject;
        // apply force to it
        ApplyForce();
        // do damage based on roaming, charging, and stomp
        wumpus.Player.GetComponent<PlayerInfo>().TakeDamage(20);
    }

    void ApplyForce(){
        // puredirection is the difference between player and self
        Vector3 PureDirection = wumpus.Player.transform.position - transform.position;
        // knockback shoves player super slightly upwards as well as in correct directions
        Vector3 KBDirection = new Vector3(PureDirection.x, 0.1f, PureDirection.z);
        // add force to player rigid body
        wumpus.playerRB.AddForce(KBDirection * wumpus.KnockBackForce * Time.deltaTime);
    }
}
