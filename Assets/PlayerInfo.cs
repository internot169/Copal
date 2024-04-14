using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

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

    public void TakeDamage(float damage)
    {
        if (TimeSinceHurt > 1)
        {
            currentHealth -= damage;
            HealthBar.value = currentHealth;
        }
        Debug.Log("Took damage, now at: " + currentHealth);
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