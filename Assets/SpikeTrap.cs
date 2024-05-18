using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    // Start is called before the first frame update

    GameObject player;

    public int damage;

    public float time_length;

    float timer = 0;
    void Start()
    {
        // find reference to player
        player = GameObject.Find("Player");

    }


    // Update is called once per frame
    void Update()
    {
        // if player is in the trap, take damage. 
        if(player_in && timer >= time_length){
            player.GetComponent<PlayerInfo>().TakeDamage(damage);
            timer = 0;
        }

        // keep timer
        timer += Time.deltaTime;
    }
}
