using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{

    // just change the moveplayer script, the checks are otherwise same. 
    public override void MovePlayer(Collider other)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().randomRoom(other);
        GameObject.Find("GameManager").GetComponent<RoomGenerator>().moveMob("Bat", base.next.roomNum);
        base.traverseMetaLogic();
    }
}
