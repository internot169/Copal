using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        // still inherits the x y z if you want to put some failsafe values from Teleporter 
        // change this to player idiots
        if(other.name == "Capsule" && isOn){
            other.transform.position = new Vector3 (x, y, z);
            SpawnBoss();
        }
    }

    void SpawnBoss(){
        Debug.Log("spawn the boss using game manager");
        // GameManager.SpawnBoss() or something
    }
}
