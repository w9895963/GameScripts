using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_ConnectBreaker : IC_Base {



    public override void OnEnable_ () {
     
        GetComponents<FixedJoint2D> ().Destroy ();
        enabled = false;
    }


    public override void OnDisable_ () {

    }


}