using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        // change this to player please
        // I believe in you Yile
        if(other.name == "Player"){
            InteractPlayer(other);
        }
    }

    public virtual void InteractPlayer(Collider other)
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
