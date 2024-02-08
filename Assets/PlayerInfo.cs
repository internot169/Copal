using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class PlayerInfo : MonoBehaviour
{
    public Slider HealthBar;
    public Text numberHealth;
    public float Health = 100;
    public double TimeSinceHurt = 0;

    private float currentHealth = 100;

    void Start()
    {
        currentHealth = Health;
    }

    public void TakeDamage(float damage)
    {
        if (TimeSinceHurt > 1)
        {
            currentHealth -= damage;
            HealthBar.value = currentHealth;
        }
    }

    void Update()
    {
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
        TimeSinceHurt += 0.25 * Time.deltaTime;
    }
}