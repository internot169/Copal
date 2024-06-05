// integration with unity
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to apply the aoe effects to unity
public class AugmentManager : MonoBehaviour
{
    // game objects that are needed
    GameObject weapon;
    GameObject SlowField;
    GameObject DroneField;
    GameObject DOTField;
    GameObject VampField;

    // start is the first thing that is called even before the first frame update
    void Start()
    {
        // find and assign each of the game objects by name
        weapon = GameObject.Find("Gun");
        SlowField = GameObject.Find("SlowApplier");
        DroneField = GameObject.Find("DamageApplier");
        DOTField = GameObject.Find("DOTApplier");
        VampField = GameObject.Find("VampApplier");
    }

    // update is called once per frame after start
    void Update()
    {
        
    }

    // methods to change the states of various augment effects on the weapon
    // methods call corresponding to raycastshoot component
    // methods act as gun flippers
    public void ChangeSlowMain(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeSlowMain(state);
    }

    public void ChangeSlowAlt(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeSlowAlt(state);
    }

    public void ChangeVampMain(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeVampMain(state);
    }

    public void ChangeDOTMain(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeDOTMain(state);
    }

    public void ChangeDOTAlt(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeDOTAlt(state);
    }

    public void ChangeVampAlt(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeVampAlt(state);
    }

    public void ChangeDroneAlt(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeDroneAlt(state);
    }

    public void ChangeDroneMain(bool state) 
    {
        weapon.GetComponent<RayCastShoot>().ChangeDroneMain(state);
    }
    
    // methods to change the active state of various AOE fields
    public void ChangeDOTField(bool state)
    {
        DOTField.SetActive(state);
    }

    public void ChangeSlowField(bool state)
    {
        SlowField.SetActive(state);
    }

    public void ChangeDroneField(bool state)
    {
        DroneField.SetActive(state);
    }

    public void ChangeVampField(bool state)
    {
        VampField.SetActive(state);
    }

    // methods to reset all main and alternate augment effects and AOE fields
    public void ResetMainAll()
    {
        // reset all main augment effects on the weapon
        ChangeDOTMain(false);
        ChangeSlowMain(false);
        ChangeVampMain(false);
        ChangeDroneMain(false);
    }

    public void ResetAltAll()
    {
        // reset all alt augment effects on the weapon
        ChangeDOTAlt(false);
        ChangeSlowAlt(false);
        ChangeVampAlt(false);
        ChangeDroneAlt(false);
    }

    public void ResetFieldAll()
    {
        // reset all aoe fields
        ChangeDOTField(false);
        ChangeSlowField(false);
        ChangeVampField(false);
        ChangeDroneField(false);
    }

    // method to modify the player's speed
    public void modifyPlayerSpeed(float newSpeed){
        gameObject.GetComponent<CrankyRigidBodyController>()._speed = newSpeed;
    }
}
