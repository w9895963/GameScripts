using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class CM_CameraControler : MonoBehaviour {

    [System.Serializable] public class Condition {
        [System.Serializable] public class OnEnabled {
            public bool enabled = false;
        }
        public OnEnabled onEnabled = new OnEnabled ();
        [System.Serializable] public class Trigger {
            public bool enabled = false;
            public Collider2D[] collider = new Collider2D[0];
            public GameObject target;
        }
        public Trigger trigger = new Trigger ();

    }
    public Condition condition = new Condition ();
    public CM_Camera.Action action = new CM_Camera.Action ();
    private List<Object> temps = new List<Object> ();
    private CM_Camera camTemp;

    private void OnEnable () {
        if (condition.trigger.enabled) {
            condition.trigger.collider.ForEach ((x) => {
                C_ColliderEvent temp = x._Ex (this).AddCollierEvent ((s) => {
                    s.objectFilter.Add (condition.trigger.target);
                    s.events.onTriggerEnter.AddListener ((c) => {
                        MainAction ();
                    });
                });
                temps.Add (temp);
            });

        }

        if (condition.onEnabled.enabled) {
            MainAction ();
        }

    }

    private void MainAction () {
        if (camTemp == null) {
            var cam = Camera.main.gameObject;
            cam.GetComponent<CM_Camera> ().DestroySelf ();
            camTemp = cam.AddComponent<CM_Camera> ();
            temps.Add (camTemp);
            camTemp.action = action;
            camTemp.enabled = true;

            temps.Add (camTemp);
        }



    }

    private void OnDisable () {
        temps.Destroy ();
    }
}