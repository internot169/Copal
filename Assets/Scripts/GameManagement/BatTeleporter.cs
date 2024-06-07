using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{

    // override the player movement script to handle custom teleporation
    public override void MovePlayer(Collider other)
    {
        StartCoroutine(ShowUI(other));
    }

    // don't need UI to pop up, so if it's active and interacting with player then just move. 
    public override void InteractPlayer(Collider other) {
        if(isOn && other.name == "Player"){
            MovePlayer(other);
        }
    }

    // show the ui about bats for a second, then move the player. 
    // We interact with gameManager here, but it's limited.
    // Breaking abstraction is mainly to allow shoot to call move()
    // and to pause/play the game and update turns
    IEnumerator ShowUI(Collider other){
        base.gameManager.BatUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        base.gameManager.BatUI.SetActive(false);
        base.gameManager.currentRoom().visited = true;
        base.gameManager.currentRoom().gameObject.SetActive(false);
        StartCoroutine(RandomRoomMove(other));
    }

    // script to move the player to a random city. 
    IEnumerator RandomRoomMove(Collider other){
        // saves the picked random room, 
        Room dest = GameObject.Find("GameManager").GetComponent<GameManager>().randomRoom();
        // does the stuff on this room that base tp does. Since this is differnt from the base destination(potentially), it's handled differently. 
        dest.gameObject.SetActive(true);
        // check the next rooms for obstacles
        dest.adjacencyCheck();
        // move the player to the destination room
        other.transform.position = new Vector3(dest.spawnLocation.position.x, dest.spawnLocation.position.y, dest.spawnLocation.position.z);
        // get the current room from game manager, then move the bat from this room. 
        GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Bat", base.gameManager.currentRoom().roomNum);
        yield return null;
    }
}
