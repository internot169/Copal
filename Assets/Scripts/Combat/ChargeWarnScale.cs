// integration with unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to apply the the warnscale effect to unity
public class ChargeWarnScale : MonoBehaviour
{
    // start is called before the first frame update
    void Start()
    {
        
    }

    // update is called once per frame
    void Update()
    {
        
    }

    // scaling the dimensions
    public void Scale(float ZDimension)
    {
        // transforming the scale as a vector using the zdimension given
        transform.localScale = new Vector3(12, 1, ZDimension);
    }
}
