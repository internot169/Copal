using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{
    Collider otherCollider;

    public void receive(bool correct){
        Debug.Log("Received");
        if (correct){
            base.MovePlayer(otherCollider);
            base.traverseMetaLogic();
            // Wumpus move away
        } else {
            GameObject.Find("GameManager").GetComponent<GameManager>().lose();
        }
    }

    public override void MovePlayer(Collider other)
    {
        otherCollider = other;
        Callback receiver;
        receiver = receive;
        GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver);
    }
}
