using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    // just a check for stomping later. 
    public bool playerIn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // updater for dealing stomp damage after. 
    void OnTriggerEnter(Collider other){
        playerIn = other.gameObject.name == "Player";
    }
}
