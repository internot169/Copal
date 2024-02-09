using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RoomManager : MonoBehaviour
{
    // temporary hardcoding, should be gotten from the GM later. 
    public String dest1 = "";
    public String dest2 = "";
    public String dest3 = "";
    public int teleporters = 3;
    public bool isBoss = false;
    public bool isBat = false;
    RoomManager[] myDestinations;
    // Start is called before the first frame update
    void Start()
    {   
        // currently hardcoded, should be able to recode this by having GameManager inject it
        myDestinations = new RoomManager[teleporters];
        myDestinations[0] = GameObject.Find(dest1).GetComponent<RoomManager>();
        myDestinations[1] = GameObject.Find(dest2).GetComponent<RoomManager>();
        myDestinations[2] = GameObject.Find(dest3).GetComponent<RoomManager>();
        // Assigns each teleporter to a destination
        int j = 0;
        // loops through each child, if the tag is a teleporter, then assign it a destination. 
        for (int i = 0; i < transform.childCount; i++){
            GameObject child = transform.GetChild(i).gameObject;
            if(child.tag == "Teleporter"){
                child.GetComponent<Teleporter>().SetRoom(myDestinations[j]);
                j++;
            }
        }
    }
 
        

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isBossRoom(){
        isBoss = true;
        // temporarily calls activate boss to make sure it works
        ActivateBoss();
    }

    public void isBatRoom(){
        isBat = true;
        ActivateBat();
    }

    public void ActivateBat(){
        // put bat code here
        // should only be called by GM upon room end
        Debug.Log("bat");
    }

    public void ActivateBoss(){
        // put boss code here
        // technically could be called off enter but up to you guys
        Debug.Log("boss");
    }

    public void Enter(Collider other){
        other.transform.position = this.transform.position;
    }
}
