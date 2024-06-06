using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base player collider class, which handles trigger enter and 
// other various things to add more clear inheritance. 
public class PlayerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // all of these should act as triggers, so detect for it. 
    void OnTriggerEnter (Collider other)
    {
        // if entered by the player then call the interact script. 
        if(other.name == "Player"){
            InteractPlayer(other);
        }
    }

    // this is an abstract class, so no definition. 
    public virtual void InteractPlayer(Collider other)
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
