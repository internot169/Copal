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

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // disable the camera
            other.GetComponentInChildren<MouseLook>().enabled = false;
            // move the player to the next room. 
            // MovePlayer(other.transform);
            // check the next rooms for obstacles
            next.adjacencyCheck();
            // tell the gamemanager that you are teleporting off this door. 
            gameManager.UpdateTp(this);
        }
    }

    public virtual void MovePlayer(Collider other){
        GameManager mg = GameObject.Find("GameManager").GetComponent<GameManager>();
        mg.roomNum = next.roomNum;
        // patch to make sure i stop clipping into the ground. 
        // we should use a different transform so its cleaner.
        other.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y+5, next.spawnLocation.position.z);
        other.GetComponent<PlayerInfo>().ChangeRoom(next.roomNum);
        traverseMetaLogic();
    }

    public void traverseMetaLogic(){
        GameManager mg = GameObject.Find("GameManager").GetComponent<GameManager>();
        mg.coins++;
    }
}
