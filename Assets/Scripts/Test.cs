using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.U2D;

public class Test : MonoBehaviour {
    public UnityEngine.Object obj;

    private void Awake () {

    }

    private void OnValidate () {
        Debug.Log (obj);
    }


    private void FixedUpdate () {

    }
}