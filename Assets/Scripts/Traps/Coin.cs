using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PlayerCollider
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override InteractPlayer(Collider other){
        // this inherits from a class that handles player interactions. 
        // When the player enters this object, the InteractPlayer script is called by the 
        // parent, which we override here. 
        // here, this statement grabs the gameobject, and the PlayerInfo script within it. 
        // It holds a reference to the gamemanager, which handles coin counts. 
        // Since it's public, I'm directly incrementing the coins when it's collected. 
        other.gameObject.GetComponent<PlayerInfo>().gameManager.coins ++;
        // then the coin destroys itself. 
        Destroy();
    }
}
