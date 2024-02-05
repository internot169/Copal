using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{
    void OnTriggerEnter (Collider other)
    {
        // still inherits the x y z if you want to put some failsafe values from Teleporter 
        // change this to player idiots
        if(other.name == "Capsule" && isOn){
            // teleports to a random location with the same y
            MoveRandomRoom(other);
        }
    }

    void MoveRandomRoom(Collider other)
    {
        // TODO: make it pick a random room and move you there
        other.transform.position = new Vector3 (Random.Range(-10f, 10f), other.transform.position.y, Random.Range(-10f, 10f));
        Debug.Log("Tps you to the main room random pos, can be changed");
    }
}
