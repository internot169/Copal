using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTeleporter : Teleporter
{
    Collider otherCollider;

    public void receive(bool correct){
        Debug.Log("Received");
        if (correct){
            Room temp = base.next;
            // Go back to the beginning
            base.next = GameObject.Find("RoomGenerator").GetComponent<RoomGenerator>().rooms[0];
            base.MovePlayer(otherCollider);
            base.next = temp;
        } else {
            GameObject.Find("GameManager").GetComponent<GameManager>().lose();
        }
    }

    public override void MovePlayer(Collider other)
    {
        otherCollider = other;
        StartCoroutine(ShowUI());
    }

    IEnumerator ShowUI(){
        base.gameManager.PitUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        base.gameManager.PitUI.SetActive(false);
        StartCoroutine(StartTrivia());
    }

    IEnumerator StartTrivia(){
        Callback receiver;
        receiver = receive;
        StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
        yield return null;
    }
}
