using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTeleporter : Teleporter
{

    public override void MovePlayer(Collider other)
    {
        // TODO: make it pick a random room and move you there
        // Prereq: GameManager
        // should be the first thing to get replaced. 
        other.transform.position = new Vector3 (Random.Range(-10f, 10f), other.transform.position.y, Random.Range(-10f, 10f));
        Debug.Log("Tps you to the main room random pos, can be changed");
    }
}
