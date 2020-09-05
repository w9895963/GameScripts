using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Hide : IC_Base {
    public GameObject target;
     void OnEnable () {

        target.Ex_Hide ();
    }
     void OnDisable () {
        target.Ex_Show ();

    }
}