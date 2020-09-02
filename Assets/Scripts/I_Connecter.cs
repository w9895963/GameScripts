using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Connecter : IC_Base {

    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Rigidbody2D rigidToConnect;
        public List<GameObject> connectTargets = new List<GameObject> (1);
        public Collider2D triggers;
        public Vector2 fixedAnchor = Vector2.zero;
    }

    [ReadOnly] public Vars vars = new Vars ();
    [System.Serializable] public class Vars {
        public FixedJoint2D fixedJointComp;
    }




    //**************************
    public override void OnEnable_ () {
        LazyConectSetup (true);
    }

    public override void OnDisable_ () {
        LazyConectSetup (false);
    }




    //*Private
    private void LazyConectSetup (bool enabled) {
        if (enabled) {
            data.tempInstance.AddIfEmpty (2, () =>
                setting.triggers.Ex_AddCollierEvent (setting.connectTargets.ToArray (),
                    onTriggerStay: (c) => {
                        vars.fixedJointComp = setting.rigidToConnect.Ex_ConnectTo (c.attachedRigidbody);
                        data.actionIndex = 0;
                        this.enabled = false;
                        LazyConectSetup (false);
                    })
            );
        } else {
            data.tempInstance.Destroy (2);
        }
    }




}