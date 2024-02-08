using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public Rigidbody bullet;
    public Transform playerObj;

    void Update()
    {

        //Shooting
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        Instantiate(bullet, bulletSpawnPoint.transform.position, playerObj.transform.rotation);
    }
}
