using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{
    // hold a reference to other collider. 
    Collider otherCollider;

    // handles interaction with trivia upon forgeting to shoot correct room
    public void receive(bool correct){
        Debug.Log("Received");
        // if trivia is correct, then move to next room, and move the wumpus. 
        if (correct){
            next.gameObject.SetActive(true);
            next.adjacencyCheck();
            otherCollider.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y, next.spawnLocation.position.z);

            GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Wumpus", base.next.roomNum);
        } else {
            // otherwise, lose. 
            GameObject.Find("GameManager").GetComponent<GameManager>().lose(true);
        }
    }

    // move the player
    public override void MovePlayer(Collider other)
    {
        // save the collider for later, get the call back from trivia and load a problem. 
        otherCollider = other;
        Callback receiver;
        receiver = receive;
        GameObject trivia = GameObject.Find("Trivia");
        StartCoroutine(trivia.GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
    }
}
