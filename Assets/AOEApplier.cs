using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AOEApplier : MonoBehaviour
{
    List<Collider> TriggerList = new List<Collider>();

    // I'm currently doing AOE in one big wave at a time, so
    // there is a chance for something to pass through AOE without
    // getting checked, which could be an issue. 

    float apply_timer;

    void Start()
    {
        apply_timer = 0;
    }

    // mark the stacks to be applied by this aoe applicator.  
    public virtual void MarkStacks(Collider other)
    {
        return;
    }
    
    //called when something enters the trigger
    void OnTriggerEnter(Collider other)
    {
        //if the object is not already in the list
        if(!TriggerList.Contains(other) && other.gameObject.tag == "Enemy")
        {
            //add the object to the list
            TriggerList.Add(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit(Collider other)
    {
        //if the object is in the list
        if(TriggerList.Contains(other))
        {
            //remove it from the list
            TriggerList.Remove(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (apply_timer > 0.125){
            foreach (Collider c in TriggerList)
            {
                MarkStacks(c);
            }
            apply_timer = 0;
        }
        apply_timer += Time.deltaTime;
    }
    
}
