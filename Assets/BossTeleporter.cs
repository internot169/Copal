using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{

    public override void MovePlayer(Collider other)
    {
        // spawn the boss or something. 
        base.MovePlayer(other);
        // should call to either gm or to room to spawn boss. 
    }
}
