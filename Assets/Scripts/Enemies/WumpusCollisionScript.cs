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
    
    void OnCollisionStay(Collision collision){
        Collider collider = collision.collider;
        GameObject player = collider.gameObject;
        Debug.Log(player.name);
        ApplyForce();
        // do damage based on roaming, charging, and stomp
        wumpus.Player.GetComponent<PlayerInfo>().TakeDamage(20);
        wumpus.Player.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, wumpus.KnockBackForce);
    }

    void ApplyForce(){
        Vector3 PureDirection = wumpus.Player.transform.position - transform.position;
        Vector3 KBDirection = new Vector3(PureDirection.x, 0.1f, PureDirection.z);
        wumpus.playerRB.AddForce(KBDirection * wumpus.KnockBackForce * Time.deltaTime);
    }
}
