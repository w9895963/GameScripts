using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Hide : IC_Base {
    public GameObject target;
    public override void OnEnable_ () {
        data.actionIndex = 0;

        target.Ex_Hide ();
    }
    public override void OnDisable_ () {
        target.Ex_Show ();

    }
}