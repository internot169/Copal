using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PlayerCollider
{
    public override void InteractPlayer(Collider other){
        // this inherits from a class that handles player interactions. 
        // When the player enters this object, the InteractPlayer script is called by the 
        // parent, which we override here. 
        // This code finds the gamemanager, then gets the script that its attached to, then increments the coins.
        GameObject.Find("GameManager").GetComponent<GameManager>().coins ++;
        // then the coin destroys itself. 
        Destroy(gameObject);
    }
}
