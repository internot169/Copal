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
        ResetMainAll();
        weapon.GetComponent<RayCastShoot>().ChangeSlowMain(state);
    }
    public void ChangeSlowAlt(bool state){
        ResetAltAll();
        weapon.GetComponent<RayCastShoot>().ChangeSlowAlt(state);
    }
    public void ChangeVampMain(bool state){
        ResetMainAll();
        weapon.GetComponent<RayCastShoot>().ChangeVampMain(state);
    }
    public void ChangeDOTMain(bool state){
        ResetMainAll();
        weapon.GetComponent<RayCastShoot>().ChangeDOTMain(state);
    }
    public void ChangeDOTAlt(bool state){
        ResetAltAll();
        weapon.GetComponent<RayCastShoot>().ChangeDOTAlt(state);
    }

    public void ChangeVampAlt(bool state){
        ResetAltAll();
        weapon.GetComponent<RayCastShoot>().ChangeVampAlt(state);
    }
    // todo: flip aoe stuff, which is easy. 
    // add movement buffs
    // add ultimates. 
    public void ResetMainAll()
    {
        // call at the start
        ChangeDOTMain(false);
        ChangeSlowMain(false);
        ChangeVampMain(false);
    }

    public void ResetAltAll(){
        ChangeDOTAlt(false);
        ChangeSlowAlt(false);
        ChangeVampAlt(false);
    }


    public void modifyPlayerSpeed(float newSpeed){
        gameObject.GetComponent<CrankyRigidBodyController>().MovementSpeed = newSpeed;
    }
}
