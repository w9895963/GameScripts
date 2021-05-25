using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable_Attack : MonoBehaviour
{
    private void Awake()
    {
        BasicEvent.OnTrigger2D_Enter.Add(gameObject, OnTrigger2DEnterAction);
    }

    public void OnTrigger2DEnterAction(Collider2D other)
    {
        throw new NotImplementedException();
    }
}
