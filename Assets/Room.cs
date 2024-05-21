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
    public string info = "";

    public GameManager gameManager;

    // TODO: Visited var
    // Why not mark this in GM?
    
    // Actually we totally should mark this in GM, so do that when 
    // we actually have MVP fully going -YS
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
                warnings += "I smell a bat.";
                adjacent[0] = true;
            }else if(doors[i] is BossTeleporter){
                warnings += "I smell a wumpus";
                adjacent[2] = true;
            }else{
                warnings += "normal tp";
            }
            // add whitespace if not the end. 
            if (i != 2){
                warnings += "\n";
            }
        }
        // update through gamemanager. 
        gameManager.UpdateWarnings(warnings);
        return adjacent;
    }

    // does all the things when you clear the room
    // I think instead of physical wall we should disable TPs but up to you. 
    public void SetRoom(){
        // please rename this thing

    }
}
