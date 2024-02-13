using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{   
    Room cur;

    HashSet<Room> visited = new HashSet<Room>();
    Room[] rooms;
    // Start is called before the first frame update
    void Start()
    {
        // make a variable that has all rooms or call. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Room PickNextRoom(){
        // make a new room
        Room newRoom = rooms[Random.Range(0, rooms.Length)];
        // check if the room has already been visited. 
        // if so, then generate a new room. 
        while (!visited.Contains(newRoom)){
            newRoom = rooms[Random.Range(0, rooms.Length)];
        }
        // add to visited list. Assume we mark the first room. 
        visited.Add(newRoom);
        // return the room back to the room object to define its teleporters. 
        return newRoom;
    }
}
