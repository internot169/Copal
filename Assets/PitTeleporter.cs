using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTeleporter : Teleporter
{

    // inherits most of the logic from teleporter, just needs to add
    // conditional for trivia. 
    public override void MovePlayer(Collider other)
    {
        StartCoroutine(callTrivia(other));
        base.traverseMetaLogic();
    }

    IEnumerator callTrivia(Collider other){
        Coroutine cd = StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2));
        if (true){
            Room temp = base.next;
            base.next = GameObject.Find("RoomGenerator").GetComponent<RoomGenerator>().rooms[0];
            base.MovePlayer(other);
            base.next = temp;
        } else {
            GameObject.Find("GameManager").GetComponent<GameManager>().lose();
        }
        return null;
    }
}
