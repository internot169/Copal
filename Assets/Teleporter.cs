using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;



public class Teleporter : MonoBehaviour
{
    public Room next;
    
    public bool isOn = true;

    void OnTriggerEnter (Collider other)
    {
        // change this to player please
        if(other.name == "Capsule" && isOn){
            // teleports to a random location with the same y
            MovePlayer(other);
        }
    }

    // helps clean up inheritance code. 
    public virtual void MovePlayer(Collider other){
        // patch to make sure i stop clipping into the ground. 
        // we should use a different transform so its cleaner. 
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y+5, next.spawnLocation.position.z);
    }
}
