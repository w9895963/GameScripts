    using System.Collections.Generic;
    using System.Collections;
    using Global;
    using UnityEngine.Events;
    using UnityEngine;

    public class C_MoveTo : MonoBehaviour {
        [System.Serializable] public class Setting {
            public Vector2 targetPosition;
            public float time;
            public AnimationCurve moveCurve = Global.Curve.ZeroOneCurve;
            public Events events = new Events ();
            [System.Serializable] public class Events {
                public UnityEvent onArrived = new UnityEvent ();
                public UnityEvent onMoving = new UnityEvent ();
                public UnityEvent onStart = new UnityEvent ();
            }
        }
        public Setting setting = new Setting ();
        [ReadOnly] public float timebegin;
        [ReadOnly] public Vector2 beginPosition;
        [ReadOnly] public Object createBy;
        [SerializeField, ReadOnly] private bool arrived = true;
        public Test test = new Test ();

        private void FixedUpdate () {
            if (!arrived) {
                float delTime = Time.time - timebegin;
                Vector2 setP = transform.position;
                if (delTime < setting.time) {
                    setP = (setting.targetPosition - beginPosition) * setting.moveCurve.Evaluate (delTime / setting.time) + beginPosition;
                } else {
                    setP = setting.targetPosition;
                }
                transform.Set2dPosition (setP);

                setting.events.onMoving.Invoke ();
                if (setP == setting.targetPosition) {
                    arrived = true;
                    setting.events.onArrived.Invoke ();
                }

            }
        }

        private void OnEnable () {
            test.move = false;
            timebegin = Time.time;
            beginPosition = transform.position;
            arrived = false;
            setting.events.onStart.Invoke ();
        }




#if UNITY_EDITOR
        private void OnValidate () {
            if (test.move) {
                test.move = false;
                timebegin = Time.time;
                beginPosition = transform.position;
                arrived = false;
            }
        }
#endif




        //*Property
        [System.Serializable]
        public class Test {
            public bool move = false;
        }


    }


    public static class Extension_C_MoveTo {

        public static C_MoveTo MoveTo (this GameObjectExMethod source, UnityAction<C_MoveTo.Setting> setup) {
            C_MoveTo comp = source.gameObject.AddComponent<C_MoveTo> ();
            comp.enabled = false;

            C_MoveTo.Setting setting = new C_MoveTo.Setting ();
            setup (setting);
            comp.setting = setting;
            comp.createBy = source.callby;

            comp.enabled = true;
            return comp;

        }

    }