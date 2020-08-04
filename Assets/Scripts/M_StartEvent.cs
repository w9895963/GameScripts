using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_StartEvent : MonoBehaviour {
    public UnityEvent startUp;
    void Start () {
        startUp.Invoke ();
    }


}