using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : PlayerCollider
{
    // Start is called before the first frame update

    public bool player_in = false;
    void Start()
    {
        
    }

    public override void InteractPlayer(Collider other)
    {
        player_in = true;
    }

    public void OnTriggerExit(Collider other)
    {
        // change this to player please
        // I believe in you Yile
        if(other.name == "Player"){
            player_in = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
