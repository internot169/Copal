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

    IEnumerator ShowUI(Collider other){
        base.gameManager.BatUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        base.gameManager.BatUI.SetActive(false);
        StartCoroutine(RandomRoomMove(other));
    }

    IEnumerator RandomRoomMove(Collider other){
        GameObject.Find("GameManager").GetComponent<GameManager>().randomRoom();
        GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Bat", base.next.roomNum);
        yield return null;
    }
}
