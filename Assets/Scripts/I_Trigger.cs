using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class I_Trigger : IC_Base {
    public Setting setting = new Setting ();

    [System.Serializable] public class Setting {
        public Collider2D trggerZone;
        public GameObject[] targets = new GameObject[0];
        public int triggerEnter = 0;
        public int triggerExit = 1;
        public bool allwaysOn = false;


    }

     void OnEnable () {
        data.actionIndex = -1;
        data.tempInstance.Add (() => {
            GameObject[] targets = setting.targets;
            return setting.trggerZone.Ex_AddCollierEvent (
                targets, onTriggerEnter: (o) => {
                    if (targets.Contains (o.gameObject)) {
                        data.actionIndex = setting.triggerEnter;
                        if (!setting.allwaysOn) enabled = false;
                        else RunOnDisable ();
                    }
                }
            );
        });
        data.tempInstance.Add (() => {
            GameObject[] targets = setting.targets;
            return setting.trggerZone.Ex_AddCollierEvent (
                targets, OnTriggerExit: (o) => {
                    if (targets.Contains (o.gameObject)) {
                        data.actionIndex = setting.triggerExit;
                        if (!setting.allwaysOn) enabled = false;
                        else RunOnDisable ();
                    }
                }
            );
        });
    }
     void OnDisable () {
        data.tempInstance.Destroy ();
    }

}