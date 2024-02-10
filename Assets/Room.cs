using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Spawn location, relative to the center of the room
    public Transform spawnLocation;
    public Teleporter[] doors;

    public string info = "";

    // TODO: Visited var
    // Why not mark this in GM?
    public bool visited = false;

    // TODO: Make an active room system
    // Why is this even a thing
    public void Awake(){
        Debug.Log(adjacencyCheck());
        // clears upon entering for testing. 
        ClearRoom();
    }


    // I think we should consider check the types of doors, and make GameManager spawn them in as we go. 
    // this also requires the next room to be preloaded with its doors, when we only need the doors of rooms
    // that we have visited. 
    public string adjacencyCheck(){
        string adjacent = "";
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].next.info != ""){
                adjacent += doors[i].next.info;
                adjacent += " ";
            }
        }
        return adjacent;
    }

    // does all the things when you clear the room
    // I think instead of physical wall we should disable TPs but up to you. 
    public void ClearRoom(){
        // please rename this thing
        transform.Find("RoomBase/teast1").gameObject.SetActive(false);
    }
}
