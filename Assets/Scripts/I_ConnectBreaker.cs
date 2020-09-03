using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_ConnectBreaker : IC_Base {



     void OnEnable () {
     
        GetComponents<FixedJoint2D> ().Destroy ();
        enabled = false;
    }


     void OnDisable () {

    }


}