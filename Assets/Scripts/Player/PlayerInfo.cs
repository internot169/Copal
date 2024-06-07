using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class PlayerInfo : MonoBehaviour
{
    // health bar
    public Slider HealthBar;
    // text element to show health
    public Text numberHealth;
    // limit of health
    public float Health = 100.0f;
    // invulnerability timer
    public double TimeSinceHurt = 0f;
    // invulnerabiility frame time. 
    public double TimeSinceHurtLimit = 0.25f;

    // internal reference for health
    private float currentHealth;

    void Start()
    {
        currentHealth = Health;
    }

    // Take damage script for other objects to interact. 
    public void TakeDamage(float damage)
    {
        // if it's been a quarter second since last tick, then
        // damage again. 
        if (TimeSinceHurt > TimeSinceHurtLimit)
        {
            // deal damage to the player, make sure it doesn't drop below 0. 
            currentHealth = Math.Max(currentHealth - damage, 0);
            HealthBar.value = currentHealth;
            numberHealth.text = currentHealth.ToString();
            Debug.Log("owch");
            Debug.Log(currentHealth);
            TimeSinceHurt = 0;
        }
    }

    // heal the player up to full hp. 
    public void Heal(float healing){
        // make sure that health doesn't heal over max hp. 
        currentHealth = Math.Min(Health, currentHealth+healing);
        // update the bar. 
        HealthBar.value = currentHealth;
        numberHealth.text = currentHealth.ToString();
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
        
        // if the current health is at 0 or lower, then check if the player loses. 
        if (currentHealth <= 0)
        {   
            // if player can respawn, heal to full hp. 
            bool respawn = !GameObject.Find("GameManager").GetComponent<GameManager>().lose();
            if (respawn){
                Heal(Health);
            }
        }
    }
}