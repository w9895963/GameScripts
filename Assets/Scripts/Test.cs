﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        Debug.Log(123);
    }

    // Update is called once per frame
    void Update () {

    }


    public void Log (string s) {
        Debug.Log (s);
    }
}