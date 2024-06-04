// integration with unity
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// class to apply the aoe effects to unity
public class AOEApplier : MonoBehaviour
{
    // keep track of all the colliders that are currently in the trigger list
    List<Collider> TriggerList = new List<Collider>();

    // timer to manage the frequency of aoe application
    float apply_timer;

    // initialize the variables
    void Start()
    {
        // start timer from 0
        apply_timer = 0;
    }

    // mark the stacks to be applied by this aoe applicator  
    public virtual void MarkStacks(Collider other)
    {
        return;
    }
    
    // called when something enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // if the object is not already in the list the goes into the statement
        if(!TriggerList.Contains(other) && other.gameObject.tag == "Enemy")
        {
            // add the object to the list
            TriggerList.Add(other);
        }
    }

    // opposite of previous method and is called when something exits the trigger
    void OnTriggerExit(Collider other)
    {
        // if the object is in the list
        if(TriggerList.Contains(other))
        {
            // remove it from the list
            TriggerList.Remove(other);
        }
    }

    // update is called once per frame
    void Update()
    {
        // the amount of time duration for the frame
        // checking if it has exceeded the threshould of 0.125 seconds
        if (apply_timer > 0.125){
            // iterate through all colliders in the triggerlist
            foreach (Collider c in TriggerList)
            {
                // call the markstacks method for each of the colliders
                MarkStacks(c);
            }
            // set the timer back to zero
            apply_timer = 0;
        }
        // increment the apply timer by the time elapsed since the last frame
        apply_timer += Time.deltaTime;
    }
}
