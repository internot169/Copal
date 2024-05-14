using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    // just a check for stomping later. 
    public bool playerIn = false;
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
        if (other.gameObject.name == "Player"){
            playerIn = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.name == "Player"){
            playerIn = false;
        }
    }
}
