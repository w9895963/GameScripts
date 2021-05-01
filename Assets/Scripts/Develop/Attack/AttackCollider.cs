using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnEnable()
    {
        UnityEvent_TriggerEnter2d.AddEvent(gameObject, TriggerAction);
    }



    private void OnDisable()
    {
        UnityEvent_TriggerEnter2d.RemoveEvent(gameObject, TriggerAction);
    }


    private void TriggerAction(Collider2D obj)
    {
        Debug.Log(obj);
    }
}
