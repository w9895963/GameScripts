using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_Timer : MonoBehaviour {
    public float timeStart;
    public float waitTime;
    public UnityEvent onTime = new UnityEvent();

    void Update () {
        if (Time.time - timeStart > waitTime) {
            onTime.Invoke ();
            Destroy (gameObject);
        }
    }


    public void WaitToCall (float time, UnityAction action) {
        timeStart = Time.time;
        waitTime = time;
        onTime.AddListener (action);
    }


}