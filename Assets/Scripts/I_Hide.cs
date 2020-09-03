using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Hide : IC_Base {
    public GameObject target;
     void OnEnable () {
        data.actionIndex = 0;

        target.Ex_Hide ();
    }
     void OnDisable () {
        target.Ex_Show ();

    }
}