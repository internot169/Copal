using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNum;
    // Spawn location, relative to the center of the room
    public Transform spawnLocation;
    // doors of the room. 
    public Teleporter[] doors;
    public int[] connectedTo;

    public GameManager gameManager;

    // TODO: Visited var
    // Why not mark this in GM?
    
    public bool visited = false;

    // TODO: Make an active room system
    // Why is this even a thing
    public void Awake(){
        // clears upon entering for testing. 
        SetRoom();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public bool[] adjacencyCheck(){
        // first place is bat, second is pit, third is boss. 
        // boolean array over string, easier to check. 
        bool[] adjacent = new bool[3];
        String warnings = "";
        for (int i = 0; i < doors.Length; i++)
        {   
            
            // Instead of infinitely cascading larger tree explorations we just check door type.
            if (doors[i] is BatTeleporter){
                // concat into the collector. 
                warnings += "My agents are near ...";
                adjacent[0] = true;
            }else if(doors[i] is BossTeleporter){
                warnings += "I'm nearby; catch me if you can.";
                adjacent[2] = true;
            }else if (doors[i] is PitTeleporter){
                warnings += "(AI Psychology bot) You feel a draft.";
                adjacent[1] = true;
            }
            // add whitespace if not the end. 
            if (i != 2){
                warnings += "\n";
            }
        }
        // update through gamemanager. 
        gameManager.logger.log(warnings);
        return adjacent;
    }

    // does all the things when you clear the room
    // I think instead of physical wall we should disable TPs but up to you. 
    public void SetRoom(){
        // please rename this thing

    }
}
