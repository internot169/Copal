using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;



public class Teleporter : PlayerCollider
{
    // reference to the next room
    public Room next;
    
    // Whether or not the teleporter is on. 
    // should be changed to false when we have detection for room end. 
    public bool isOn = true;

    // override the interact player script to do teleportation instead. 
    public override void InteractPlayer(Collider other)
    {
        if(isOn){
            // move the player to the next room. 
            MovePlayer(other);
            // check the next rooms for obstacles
            next.adjacencyCheck();
        }
    }

    // helps clean up inheritance code. 
    public virtual void MovePlayer(Collider other){
        // patch to make sure i stop clipping into the ground. 
        // we should use a different transform so its cleaner. 
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y+5, next.spawnLocation.position.z);
        other.GetComponent<PlayerInfo>().ChangeRoom(next.roomNum);
    }
}
