using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;



public class Teleporter : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public int z = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        // change this to player idiots
        if(other.name == "Capsule"){
            // teleports to a random location with the same y
            other.transform.position = new Vector3 (x, y, z);
        }
    }
}
