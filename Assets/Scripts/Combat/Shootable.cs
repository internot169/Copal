using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour
{
    [Header("Shootable")]
    //The box's current health point total
    public int currentHealth = 3;

    // virtual because Wumpus overrides it for parent inactive instead of self
    public virtual void Damage(int damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            //if health has fallen below zero, deactivate it 
            gameObject.SetActive(false);
        }
    }
}