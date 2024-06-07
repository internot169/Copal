using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{

    // just change the moveplayer script, the checks are otherwise same. 
    public override void MovePlayer(Collider other)
    {
        StartCoroutine(ShowUI(other));
    }

    public override void InteractPlayer(Collider other) {
        if(isOn && other.name == "Player"){
            MovePlayer(other);
        }
    }

    IEnumerator ShowUI(Collider other){
        base.gameManager.BatUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        base.gameManager.BatUI.SetActive(false);
        StartCoroutine(RandomRoomMove(other));
    }

    IEnumerator RandomRoomMove(Collider other){
        Room dest = GameObject.Find("GameManager").GetComponent<GameManager>().randomRoom();
        dest.gameObject.SetActive(true);
        // check the next rooms for obstacles
        dest.adjacencyCheck();
        other.transform.position = new Vector3(dest.spawnLocation.position.x, dest.spawnLocation.position.y+5, dest.spawnLocation.position.z);
        GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Bat", base.next.roomNum);
        yield return null;
    }
}
