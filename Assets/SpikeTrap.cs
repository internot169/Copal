using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    // Start is called before the first frame update

    GameObject player;
    void Start()
    {
        // find reference to player
        player = GameObject.Find("Player");
    }


    // Update is called once per frame
    void Update()
    {
        // if player is in the trap, take damage. 
        if(player_in){
            player.GetComponent<PlayerInfo>().TakeDamage(10);
        }
        
    }
}
