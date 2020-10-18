using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Function;
using UnityEngine;

public class CM_Camera : MonoBehaviour {
    [System.Serializable] public class Action {
        [System.Serializable] public class MoveTo {
            public bool enabled = false;
            public GameObject target;
            public float time = 1f;
            public AnimationCurve curve = Curve.ZeroOne;
        }
        public MoveTo moveTo = new MoveTo ();
        [System.Serializable] public class Zoom {
            public bool enabled = false;
            public float size = 0;
            public float time = 0;
            public AnimationCurve curve = Curve.Default;
        }
        public Zoom zoom = new Zoom ();
        [System.Serializable] public class FollowPlayer {
            public bool enabled = false;
            public C1_Follow.Setting Setting = new C1_Follow.Setting ();
        }

        public FollowPlayer follow = new FollowPlayer ();

    }
    public Action action = new Action ();

    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();
    private bool createInInspector = false;

    //*---------------

    private void Awake () {
        if (!createInInspector) {
            enabled = false;
        }

    }
    private void OnValidate () {
        createInInspector = true;

    }
    private void OnEnable () {
        if (action.moveTo.enabled) {
            C_MoveTo temp = null;
            temp = Camera.main.gameObject._Ex (this).MoveTo ((s) => {
                s.require.targetPosition = action.moveTo.target.GetPosition2d ();
                s.require.time = action.moveTo.time;
                s.optional.moveCurve = action.moveTo.curve;
                s.events.onArrived.AddListener (() => {
                    temp.Destroy ();
                });
            });
            temps.Add (temp);
        }


        if (action.zoom.enabled) {
            temps.Add (Fn (this).AnimateData ((s) => {
                var set = s.useFloatVersion;
                var require = set.require;
                require.time = action.zoom.time;
                require.valueStart = Camera.main.orthographicSize;
                require.valueEnd = action.zoom.size;
                set.optional.useCurve = true;
                set.optional.curve = action.zoom.curve;
                var events = set.events;
                events.onAnimation.AddListener ((f) => {
                    CameraCtrl.Size = f;
                });

            }));

        }

        if (action.follow.enabled) {
            temps.Add (Camera.main.gameObject._Ex (this).Follow ((s) => {
                return action.follow.Setting;

            }));
        }
    }



    private void OnDisable () {
        temps.Destroy ();
    }




}

namespace Global {
    public static class CameraCtrl {
        public static GameObject MainCam => Camera.main.gameObject;
        public static void CommitAction (CM_Camera.Action setting) {
            CM_Camera comp = MainCam.GetComponent<CM_Camera> ();
            // comp.enabled = false;
            comp.action = setting;
            comp.enabled = true;
        }


        public static float Size {
            set {
                MainCam.GetComponent<Camera> ().orthographicSize = value;
                GlobalObject.IndicatorCamera.GetComponent<Camera> ().orthographicSize = value;
            }
        }

    }
}