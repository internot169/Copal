using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : PlayerCollider
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void InteractPlayer(Collider other)
    {
        other.transform.gameObject.GetComponent<PlayerInfo>().TakeDamage(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
