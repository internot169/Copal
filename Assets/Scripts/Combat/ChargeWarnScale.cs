using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWarnScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scale(float ZDimension)
    {
        transform.localScale = new Vector3(12, 1, ZDimension);
    }
}
