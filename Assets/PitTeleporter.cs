using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTeleporter : Teleporter
{

    // inherits most of the logic from teleporter, just needs to add
    // conditional for trivia. 
    public override void MovePlayer(Collider other)
    {
        // call trivia thing whatever we want to do 
        Debug.Log("Pit room call");

        // leaving in if condition for trivia pass. 
        if (true){
            base.MovePlayer(other);
        }
    }
}
