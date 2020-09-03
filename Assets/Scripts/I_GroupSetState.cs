using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_GroupSetState : IC_Base {
    public string state;
    public IG_GroupCore group;
    private void OnEnable () {
        group.Changestate (state);
    }
}