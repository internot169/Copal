using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowApplier : AOEApplier
{
    public override void MarkStacks(Collider other)
    {
        GameObject gameObject = other.gameObject;
        // apply slow logic for now. 
        Debug.Log(gameObject.name);
        gameObject.GetComponent<Enemy>().MarkSlows();
    }
}
