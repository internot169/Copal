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

    public GameManager gameManager;

    public void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // override the interact player script to do teleportation instead. 
    public override void InteractPlayer(Collider other)
    {
        // show the UI. 
        if(isOn && other.name == "Player"){
            gameManager.pauseGame();
            // tell the gamemanager that you are teleporting off this door. 
            // not using messaging because I'm unfamiliar, since I come from java. 
            // I think its easier to update a pointer. 
            gameManager.UpdateTp(this);
            if (this is BossTeleporter || this is PitTeleporter || this is BatTeleporter){
                gameManager.move(next.roomNum, false);
                // Set inactive later
            } else {
                gameManager.move(next.roomNum, true);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        InteractPlayer(other);
    }

    // all teleporters deal with moving the player, so this will change the player and do everything. 
    public virtual void MovePlayer(Collider other){
        // patch to make sure i stop clipping into the ground. 
        // we should use a different transform so its cleaner.
        // move the player to the room
        // all rooms are pregenerated in the scene, easier to move around. 
        // would like to point out that diff scenes are possible, but loading times would be much slower
        // so in a sense, by using enclosed rooms we're utilizing unity's LOD system to save time. 
        // the LOD system stops rendering anything not in the player's LOS, which means that the walls prevents other 
        // rooms from being rendered, but means that we can save time on room changes when compared to scene.
        // only issue being clipping into other rooms, but we generally handle that gracefully. 

        next.gameObject.SetActive(true);
        // check the next rooms for obstacles
        next.adjacencyCheck();
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y+5, next.spawnLocation.position.z);
    }
}
