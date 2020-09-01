using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_ConnectBreaker : IC_Base {
    public int success = 0;
    public int fail = 1;



    public override void EnableAction () {
        I_Connecter[] objs = behaviour.GetConnects<I_Connecter> ();

        if (objs.Length > 0) {
            objs.ForEach ((con) => {
                I_Connecter comp = con as I_Connecter;
                FixedJoint2D joint = comp.vars.fixedJointComp;

                if (joint) {
                    joint.Destroy ();
                    data.actionIndex=success;
                } else {
                     data.actionIndex=fail;
                }
                this.enabled = false;
            });

        }

    }


    public override void DisableAction () {

    }


}