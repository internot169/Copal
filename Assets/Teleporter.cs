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
        // change this to player idiots
        if(other.name == "Capsule" && isOn){
            // teleports to a random location with the same y
            other.transform.position = next.spawnLocation.position;
        }
    }
}
