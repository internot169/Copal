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
            GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Wumpus", base.next.roomNum);
        } else {
            GameObject.Find("GameManager").GetComponent<GameManager>().lose();
        }
    }

    public override void MovePlayer(Collider other)
    {
        otherCollider = other;
        Callback receiver;
        receiver = receive;
        GameObject trivia = GameObject.Find("Trivia");
        StartCoroutine(trivia.GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
    }
}
