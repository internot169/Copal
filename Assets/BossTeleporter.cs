using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{

    // move the player, but add a boss calling script for that. 
    public override void MovePlayer(Collider other)
    {
        // spawn the boss
        GameObject.Find("GameManager").GetComponent<GameManager>().bossFight();
        base.MovePlayer(other);
        base.traverseMetaLogic();
    }
}
