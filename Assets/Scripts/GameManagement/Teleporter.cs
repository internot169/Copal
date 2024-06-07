using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : PlayerCollider
{
    // reference to the next room
    public Room next;
    
    // Whether or not the teleporter is on. 
    // should be changed to false when we have detection for room end. 
    public bool isOn = true;

    // reference to the game manager. 
    public GameManager gameManager;

    public void Awake(){
        // find and assign game manager. 
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // assign the numbers that tell the destination of the room. 
    public void AssignNumbers(){
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro meshes in texts){
            Debug.Log(meshes);
            Debug.Log(next.roomNum);
            meshes.text = "" + next.roomNum;
            
        }
    }

    // override the interact player script to do teleportation instead. 
    public override void InteractPlayer(Collider other)
    {
        // show the UI. 
        if(isOn && other.name == "Player"){
            gameManager.pauseGame();
            // tell the gamemanager that you are teleporting off this door. 
            // custom handling for the boss and pit teleporters. 
            // We interact with gameManager here, but it's limited.
            // Breaking abstraction is mainly to allow shoot to call move()
            // and to pause/play the game and update turns
            if (this is BossTeleporter || this is PitTeleporter){
                gameManager.move(next.roomNum, false, this);
                // Set inactive later
            } else {
                // otherwise, move normally. 
                gameManager.move(next.roomNum, true, this);
            }
        }
    }

    // on the trigger enter, call interact player. 
    void OnTriggerEnter(Collider other)
    {
        InteractPlayer(other);
    }

    // all teleporters deal with moving the player, so this will change the player and do everything. 
    public virtual void MovePlayer(Collider other){
        // activate the next room. 
        next.gameObject.SetActive(true);
        // check the next rooms for obstacles
        next.adjacencyCheck();
        // move the player to the spawn location. 
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y, next.spawnLocation.position.z);
    }
}
