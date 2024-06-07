using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTeleporter : Teleporter
{
    // hold a reference to the other collider.
    Collider otherCollider;

    // recieves the correct answer from the trivia object. 
    public void receive(bool correct){
        Debug.Log("Received");
        Debug.Log(correct);
        // if the answer is correct, send to the right room and treat normally. 
        if (correct){
            Room temp = base.next;
            // Go back to the beginning
            base.next = GameObject.Find("GameManager").GetComponent<RoomGenerator>().rooms[0];
            next.gameObject.SetActive(true);
            next.adjacencyCheck();
            otherCollider.transform.position = new Vector3(next.spawnLocation.position.x, next.spawnLocation.position.y, next.spawnLocation.position.z);

            base.next = temp;
        } else {
            // otherwise, lose a life. 
            GameObject.Find("GameManager").GetComponent<GameManager>().lose(true);
        }
    }

    public override void MovePlayer(Collider other)
    {   
        // hold reference to other collider. 
        otherCollider = other;
        StartCoroutine(ShowUI());
    }

    // show the UI for pits for a bit
    IEnumerator ShowUI(){
        base.gameManager.PitUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        base.gameManager.PitUI.SetActive(false);
        // start the trivia. 
        StartCoroutine(StartTrivia());
    }

    IEnumerator StartTrivia(){
        Callback receiver;
        receiver = receive;
        StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
        yield return null;
    }
}
