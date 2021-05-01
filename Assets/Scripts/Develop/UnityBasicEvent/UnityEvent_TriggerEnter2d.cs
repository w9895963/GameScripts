using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnityEvent_TriggerEnter2d : MonoBehaviour
{
    private Action<Collider2D> action;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (action != null)
            action.Invoke(other);
    }


    public static void AddEvent(GameObject gameObject, Action<Collider2D> action)
    {

        UnityEvent_TriggerEnter2d com = gameObject.GetComponent<UnityEvent_TriggerEnter2d>();
        if (com == null)
        {
            com = gameObject.AddComponent<UnityEvent_TriggerEnter2d>();
        }
        com.action += action;
    }
    public static void RemoveEvent(GameObject gameObject, Action<Collider2D> action)
    {
        UnityEvent_TriggerEnter2d com = gameObject.GetComponent<UnityEvent_TriggerEnter2d>();
        if (com != null)
        {
            com.action -= action;
        }
    }
}
