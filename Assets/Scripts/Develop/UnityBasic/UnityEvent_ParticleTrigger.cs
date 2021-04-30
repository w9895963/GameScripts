using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEvent_ParticleTrigger : MonoBehaviour
{
    private Action action;


    private void OnParticleTrigger()
    {
        action.Invoke();
    }


    public static void AddEvent(GameObject gameObject, Action action)
    {

        UnityEvent_ParticleTrigger com = gameObject.GetComponent<UnityEvent_ParticleTrigger>();
        if (com == null)
        {
            com = gameObject.AddComponent<UnityEvent_ParticleTrigger>();
        }
        com.action += action;
    }
    public static void RemoveEvent(GameObject gameObject, Action action)
    {
        UnityEvent_ParticleTrigger com = gameObject.GetComponent<UnityEvent_ParticleTrigger>();
        if (com != null)
        {
            com.action -= action;
        }
    }



}



