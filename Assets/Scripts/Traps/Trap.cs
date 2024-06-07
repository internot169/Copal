using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : PlayerCollider
{
    // Start is called before the first frame update

    public bool player_in = false;
    // if interacting with player, then player is inside. 
    // Allow children to handle differently based on case. 
    public override void InteractPlayer(Collider other)
    {
        player_in = true;
    }

    // introduce detection for when the player leaves the trap. 

    public void OnTriggerExit(Collider other)
    {
        // on exit, mark player as not inside. 
        if(other.name == "Player"){
            player_in = false;
        }
    }
}
