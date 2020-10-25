using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEvent : MonoBehaviour {
    public UnityEvent onUpdate = new UnityEvent ();
    private void Update () {
        onUpdate.Invoke ();
    }


}