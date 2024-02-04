using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        // still inherits the x y z if you want to put some failsafe values from Teleporter 
        // change this to player idiots
        if(other.name == "Capsule"){
            // teleports to a random location with the same y
            MoveRandomRoom(other);
        }
    }

    void MoveRandomRoom(Collider other)
    {
        other.transform.position = new Vector3 (Random.Range(-10f, 10f), other.transform.position.y, Random.Range(-10f, 10f));
        Debug.Log("Tps you to the main room random pos, can be changed");
    }
}
