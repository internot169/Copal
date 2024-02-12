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
        // clears upon entering for testing. 
        ClearRoom();
    }


    public bool[] adjacencyCheck(){
        // first place is bat, second is pit, third is boss. 
        // boolean array over string, easier to check. 
        bool[] adjacent = new bool[3];
        // divider
        Debug.Log("--------");
        for (int i = 0; i < doors.Length; i++)
        {   
            
            // Instead of infinitely cascading larger tree explorations we just check door type.
            if (doors[i] is BatTeleporter){
                Debug.Log("I hear wings");
                adjacent[0] = true;
            }else if(doors[i] is BossTeleporter){
                Debug.Log("I smell a wumpus");
                adjacent[2] = true;
            }else{
                Debug.Log("normal tp");
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
