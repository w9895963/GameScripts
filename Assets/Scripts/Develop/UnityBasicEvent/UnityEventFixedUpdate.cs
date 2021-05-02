using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEventFixedUpdate : MonoBehaviour
{
    private Action action;


    private void FixedUpdate()
    {
        action.Invoke();
    }


    public static void AddEvent(GameObject gameObject, Action action)
    {

        UnityEventFixedUpdate com = gameObject.GetComponent<UnityEventFixedUpdate>();
        if (com == null)
        {
            com = gameObject.AddComponent<UnityEventFixedUpdate>();
        }
        com.action += action;
    }
    public static void RemoveEvent(GameObject gameObject, Action action)
    {
        UnityEventFixedUpdate com = gameObject.GetComponent<UnityEventFixedUpdate>();
        if (com != null)
        {
            com.action -= action;
        }
    }
}
