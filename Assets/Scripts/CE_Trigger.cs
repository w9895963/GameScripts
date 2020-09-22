using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class CE_Trigger : MonoBehaviour {

    [System.Serializable] public class CameraCs {
        public MoveTo moveTo = new MoveTo ();

        [System.Serializable] public class MoveTo {
            public bool enabled = false;
            public GameObject target;
            public float time;
            public AnimationCurve curve = Curve.ZeroOneCurve;
        }
    }

    public List<CameraCs> cameraAction = new List<CameraCs> ();

    public List<Object> temps = new List<Object> ();



    private void OnEnable () {
        cameraAction.ForEach ((x) => {
            if (x.moveTo.enabled) {
                Camera.main.gameObject._ExMethod (this).MoveTo ((s) => {
                    
                });
            }
        });

    }
    private void OnDisable () {
        temps.Destroy ();
    }
}