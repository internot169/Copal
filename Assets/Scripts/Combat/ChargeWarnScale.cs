// integration with unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to apply the the warnscale effect to unity
public class ChargeWarnScale : MonoBehaviour
{

    // scaling the dimensions
    public void Scale(float ZDimension)
    {
        // transforming the scale as a vector using the zdimension given
        transform.localScale = new Vector3(12, 1, ZDimension);
    }
}
