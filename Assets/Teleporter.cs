using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;



public class Teleporter : MonoBehaviour
{
    RoomManager nextRoom;

    public bool isOn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // set destination room.
    public void SetRoom(RoomManager room){
        nextRoom = room;
    }

    void OnTriggerEnter (Collider other)
    {
        // change this to player idiots
        if(other.name == "Capsule" && isOn){
            // pick a room using GM which should return this
            ActivateUnique(nextRoom);
            nextRoom.Enter(other);
        }
    }

    // normal teleporters don't need anything
    public virtual void ActivateUnique(RoomManager nextRoom){
        return;
    }
}
