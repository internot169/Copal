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

    public GameObject ui;

    public GameManager gameManager;

    public void Awake(){
        ui = GameObject.Find("ArrowUI");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // override the interact player script to do teleportation instead. 
    public override void InteractPlayer(Collider other)
    {
        // show the UI. 
        if(isOn){
            // we need to free the player when we open this ui or something. 
            ui.SetActive(true);

            // unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // disable the camera
            other.GetComponentInChildren<MouseLook>().enabled = false;
            // move the player to the next room. 
            // MovePlayer(other.transform);
            // check the next rooms for obstacles
            next.adjacencyCheck();
            // tell the gamemanager that you are teleporting off this door. 
            // not using messaging because I'm unfamiliar, since I come from java. 
            // I think its easier to update a pointer. 
            gameManager.UpdateTp(this);
            gameManager.move(next.roomNum);
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
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y+5, next.spawnLocation.position.z);
    }
}
