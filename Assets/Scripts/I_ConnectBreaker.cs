using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_ConnectBreaker : IC_Base {
    public int success = 0;
    public int fail = 1;



    public override void EnableAction () {


        I_Connecter comp = behaviour.dataConnect[0] as I_Connecter;
        FixedJoint2D joint = comp.vars.fixedJointComp;

        if (joint) {
            joint.Destroy ();
            data.exitIndex = success;
        } else {
            data.exitIndex = fail;
        }

        this.enabled = false;

    }


    public override void DisableAction () {

    }


}