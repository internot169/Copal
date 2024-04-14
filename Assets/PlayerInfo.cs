using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class PlayerInfo : MonoBehaviour
{
    public int roomNum;
    public Slider HealthBar;
    public Text numberHealth;
    public Text currentRoom;
    public float Health = 100;
    public double TimeSinceHurt = 0;

    private float currentHealth = 100;

    void Start()
    {
        currentHealth = Health;
        ChangeRoom(1);
    }

    public void ChangeRoom(int i){
        roomNum = i;
        currentRoom.text = "Room " + roomNum.ToString();
    }

    // Take damage script for other objects to interact. 
    public void TakeDamage(float damage)
    {
        // if it's been a quarter second since last tick, then
        // damage again. 
        if (TimeSinceHurt > 0.25)
        {
            currentHealth = Math.Max(currentHealth - damage, 0);
            HealthBar.value = currentHealth;
            numberHealth.text = currentHealth.ToString();
            Debug.Log("owch");
            TimeSinceHurt = 0;
        }
    }

    // continuously damage the player
    void Update()
    {
        // testing script for manual damaging. 
        if (TimeSinceHurt > 0.25)
        {
            if (Input.GetKey("m"))
            {
                currentHealth -= 1;
                HealthBar.value = currentHealth;
                numberHealth.text = currentHealth.ToString();
                Debug.Log("owch");
                TimeSinceHurt = 0;
            }
        }
        // increment timer. 
        TimeSinceHurt += 0.25 * Time.deltaTime;
    }
}