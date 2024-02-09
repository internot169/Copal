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
    public bool visited = false;

    // TODO: Make an active room system
    public void Awake(){
        Debug.Log(adjacencyCheck());
    }

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
}
