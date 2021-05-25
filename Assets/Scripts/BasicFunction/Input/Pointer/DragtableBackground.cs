using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragtableBackground : MonoBehaviour
{
    void Start()
    {
        BasicEvent.OnPointerDrag.Add(gameObject, (d) =>
        {
            d.screenPosition.Log();
        });
    }


}
