using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnEnable()
    {
        BasicEvent.OnTrigger2D_Enter.Add(gameObject, TriggerAction);
    }



    private void OnDisable()
    {
        BasicEvent.OnTrigger2D_Enter.Remove(gameObject, TriggerAction);
    }


    private void TriggerAction(Collider2D obj)
    {
        Debug.Log(obj);
    }
}
