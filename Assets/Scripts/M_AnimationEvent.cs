using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_AnimationEvent : MonoBehaviour {
    public UnityEvent[] events;
    public void CallEvent (int i) {
        if (events.Length > i) {
            events[i].Invoke ();
        }

    }


}