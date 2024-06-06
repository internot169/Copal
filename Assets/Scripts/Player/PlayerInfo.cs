using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class PlayerInfo : MonoBehaviour
{
    public Slider HealthBar;
    public Text numberHealth;
    public float Health = 100.0f;
    public double TimeSinceHurt = 0f;
    public double TimeSinceHurtLimit = 0.25f;

    private float currentHealth;

    private GameObject slowField;

    private GameObject DOTField;

    void Start()
    {
        currentHealth = Health;
        DOTField = GameObject.Find("DOTApplier");
        slowField = GameObject.Find("SlowApplier");
    }

    // Take damage script for other objects to interact. 
    public void TakeDamage(float damage)
    {
        // if it's been a quarter second since last tick, then
        // damage again. 
        if (TimeSinceHurt > TimeSinceHurtLimit)
        {
            currentHealth = Math.Max(currentHealth - damage, 0);
            HealthBar.value = currentHealth;
            numberHealth.text = currentHealth.ToString();
            Debug.Log("owch");
            Debug.Log(currentHealth);
            TimeSinceHurt = 0;
        }
    }

    public void Heal(float healing){
        currentHealth = Math.Min(100, currentHealth+healing);
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

        if (currentHealth <= 0)
        {
            bool respawn = !GameObject.Find("GameManager").GetComponent<GameManager>().lose();
            if (respawn){
                Heal(Health);
            }
        }
    }
}