using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTApplier : AOEApplier
{
    public override void MarkStacks(Collider other)
    {
        GameObject gameObject = other.gameObject;
        // apply slow logic for now. 
        gameObject.GetComponent<Enemy>().MarkBurns();
    }
}
