using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.U2D;

public class Test : MonoBehaviour {
    public Object obj;
    void Start () {

    }

    void Update () {


    }
    private void OnValidate () {
        Debug.Log (obj);
    }
}