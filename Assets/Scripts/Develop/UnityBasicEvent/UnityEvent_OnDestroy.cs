using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using System;

public class UnityEvent_OnDestroy : MonoBehaviour
{
    private Action action;

    private void OnDestroy()
    {
        action.Invoke();
    }
   

    public static void AddEvent(GameObject gameObject, Action action)
    {

        UnityEvent_OnDestroy com = gameObject.GetComponent<UnityEvent_OnDestroy>();
        if (com == null)
        {
            com = gameObject.AddComponent<UnityEvent_OnDestroy>();
        }
        com.action += action;
    }
    public static void RemoveEvent(GameObject gameObject, Action action)
    {
        UnityEvent_OnDestroy com = gameObject.GetComponent<UnityEvent_OnDestroy>();
        if (com != null)
        {
            com.action -= action;
        }
    }



}
