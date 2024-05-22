using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
       weapon = GameObject.Find("Gun");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Example Flipper

    public void ChangeSlowMain(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeSlowMain(state);
    }
    // do also for slow alt, vamp main, dot main, dot alt as well. 
    // @rishaypuri


    public void modifyPlayerSpeed(float newSpeed){
        gameObject.GetComponent<CrankyRigidBodyController>().MovementSpeed = newSpeed;
    }
}
