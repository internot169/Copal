using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{

    // just change the moveplayer script, the checks are otherwise same. 
    public override void MovePlayer(Collider other)
    {
        // TODO: make it pick a random room and move you there
        // Prereq: GameManager
        // should be the first thing to get replaced. 
        GameObject.Find("GameManager").GetComponent<GameManager>().randomRoom(other);
    }
}
