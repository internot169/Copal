using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{

    // so basically teleporter always calls an activate unique function
    public override void ActivateUnique(RoomManager nextRoom){
        nextRoom.isBossRoom();
    void OnTriggerEnter (Collider other)
    {
        // still inherits the x y z if you want to put some failsafe values from Teleporter 
        // change this to player idiots
        if(other.name == "Capsule" && isOn){
            other.transform.position = next.spawnLocation.position;
            SpawnBoss();
        }
    }

    void SpawnBoss(){
        Debug.Log("spawn the boss using game manager");
        // GameManager.SpawnBoss() or something
    }
}
