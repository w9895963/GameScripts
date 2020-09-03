using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_SimpleInspector : IC_Base {
    [SerializeField]
    private IC_Base[] next = new IC_Base[0];

    public void OnValidate () {
        behaviour.onFinish.ExpendTo (0) [0].setEnable = next.ToList ();
    }
}