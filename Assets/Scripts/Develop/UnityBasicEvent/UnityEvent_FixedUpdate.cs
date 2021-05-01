using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEvent_FixedUpdate : MonoBehaviour
{
    private Action action;


    private void FixedUpdate()
    {
        action.Invoke();
    }


    public static void AddEvent(GameObject gameObject, Action action)
    {

        UnityEvent_FixedUpdate com = gameObject.GetComponent<UnityEvent_FixedUpdate>();
        if (com == null)
        {
            com = gameObject.AddComponent<UnityEvent_FixedUpdate>();
        }
        com.action += action;
    }
    public static void RemoveEvent(GameObject gameObject, Action action)
    {
        UnityEvent_FixedUpdate com = gameObject.GetComponent<UnityEvent_FixedUpdate>();
        if (com != null)
        {
            com.action -= action;
        }
    }
}
