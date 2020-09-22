using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using UnityEngine;

public class Ctrl_Camera : MonoBehaviour {
    [System.Serializable] public class Condition {
        [System.Serializable] public class OnEnabled {
            public bool enabled = false;
        }
        public OnEnabled onEnabled = new OnEnabled ();
        [System.Serializable] public class Trigger {
            public bool enabled = false;
            public Collider2D collider;

        }
        public Trigger trigger = new Trigger ();

    }
    public Condition condition = new Condition ();
    [System.Serializable] public class Action {
        [System.Serializable] public class MoveTo {
            public bool enabled = false;
            public GameObject target;
            public float time = 1f;
            public AnimationCurve curve = Curve.ZeroOneCurve;
        }
        public MoveTo moveTo = new MoveTo ();
        [System.Serializable] public class Zoom {
            public bool enabled = false;
            public float size = 0;
            public float time = 0;
            public AnimationCurve curve = Curve.Default;
        }
        public Zoom zoom = new Zoom ();

    }
    public Action action = new Action ();

    [System.Serializable] public class Follow {
        public bool enabled = false;
        public C1_Follow.Setting setting = new C1_Follow.Setting ();

    }

    public Follow follow = new Follow ();
    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();

    //*---------------
    private void OnEnable () {
        if (condition.trigger.enabled) {
            temps.Add (condition.trigger.collider._Ex (this).AddCollierEvent ((s) => {
                s.objectFilter.Add (GlobalObject.MainCharactor.gameObject);
                s.events.onTriggerEnter.AddListener ((c) => {
                    MainAction ();
                });
            }));



        }

        if (condition.onEnabled.enabled) {
            MainAction ();
        }
    }



    private void OnDisable () {
        temps.Destroy ();
    }



    //* Private Method
    private void MainAction () {
        if (action.moveTo.enabled) {
            C_MoveTo temp = null;
            temp = Camera.main.gameObject._ExMethod (this).MoveTo ((s) => {
                s.targetPosition = action.moveTo.target.Get2dPosition ();
                s.time = action.moveTo.time;
                s.moveCurve = action.moveTo.curve;
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
                    Camera.main.orthographicSize = f;
                });

            }));

        }
    }



    //* Public Method
    public static void FollowCharactor () {

    }



}

namespace Global {
    public static class CameraCtrl {
        public static void ShowSystemCursor (bool enabled) {
            UnityEngine.Cursor.visible = enabled;
        }
    }
}