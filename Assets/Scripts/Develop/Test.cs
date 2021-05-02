using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        Action<Collision2D> action = (d) =>
        {
            Debug.Log(d);

        };
        BasicEvent.EventAction.Add(gameObject, BasicEvent.EventType.OnCollisionEnter2D, action);
        Action action2 = null;
        BasicEvent.EventAction.Add(gameObject, BasicEvent.EventType.FixedUpdate, action2);
    }
    private void Reset()
    {

    }




}