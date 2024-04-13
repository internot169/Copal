using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void modifyPlayerSpeed(float newSpeed){
        gameObject.GetComponent<CrankyRigidBodyController>().MovementSpeed = newSpeed;
    }

    public void modifyVampirism(Boolean hasVampirism){
        // add to gun script later. 
        return;
    }
}
