using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    GameObject weapon;

    GameObject SlowField;
    GameObject DroneField;

    GameObject DOTField;

    GameObject VampField;
    // Start is called before the first frame update
    void Start()
    {
       weapon = GameObject.Find("Gun");
       SlowField = GameObject.Find("SlowApplier");
       DroneField = GameObject.Find("DamageApplier");
       DOTField = GameObject.Find("DOTApplier");
       VampField = GameObject.Find("VampApplier");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // gun flippers
    public void ChangeSlowMain(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeSlowMain(state);
    }
    public void ChangeSlowAlt(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeSlowAlt(state);
    }
    public void ChangeVampMain(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeVampMain(state);
    }
    public void ChangeDOTMain(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeDOTMain(state);
    }
    public void ChangeDOTAlt(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeDOTAlt(state);
    }

    public void ChangeVampAlt(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeVampAlt(state);
    }

    public void ChangeDroneAlt(bool state){
        weapon.GetComponent<RayCastShoot>().ChangeDroneAlt(state);
    }

    public void ChangeDroneMain(bool state){

        weapon.GetComponent<RayCastShoot>().ChangeDroneMain(state);
    }
    
    // AOE Flippers
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

    // add movement buffs
    // add ultimates. 


    // convenient resetters
    public void ResetMainAll()
    {
        // call at the start
        ChangeDOTMain(false);
        ChangeSlowMain(false);
        ChangeVampMain(false);
        ChangeDroneMain(false);
    }

    public void ResetAltAll(){
        ChangeDOTAlt(false);
        ChangeSlowAlt(false);
        ChangeVampAlt(false);
        ChangeDroneAlt(false);
    }

    public void ResetFieldAll(){
        ChangeDOTField(false);
        ChangeSlowField(false);
        ChangeVampField(false);
        ChangeDroneField(false);
    }


    public void modifyPlayerSpeed(float newSpeed){
        gameObject.GetComponent<CrankyRigidBodyController>().MovementSpeed = newSpeed;
    }
}
