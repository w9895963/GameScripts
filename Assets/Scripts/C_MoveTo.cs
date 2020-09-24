    using System.Collections.Generic;
    using System.Collections;
    using Global;
    using UnityEngine.Events;
    using UnityEngine;

    public class C_MoveTo : MonoBehaviour {
        [System.Serializable] public class Setting {
            [System.Serializable] public class Require {
                public Vector2 targetPosition;
                public float time;
            }
            public Require require = new Require ();
            [System.Serializable] public class Optional {
                public float maxAccelerate = 50;
                public AnimationCurve moveCurve = Global.Curve.ZeroOne;
            }
            public Optional optional = new Optional ();
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
        private Vector2? lastPosition;

        private void FixedUpdate () {
            if (!arrived) {
                Setting s = setting;
                float time = Time.time - timebegin;
                Vector2 currP = gameObject.Get2dPosition ();
                Vector2 lastP = lastPosition.NotNull () ? lastPosition.ToVector2 () : currP;
                Vector2 targetPosition = s.require.targetPosition;
                Vector2 v = targetPosition - beginPosition;
                Vector2 normal = v.Rotate (90).normalized;
                float dist = v.magnitude;
                float range;
                Vector2 CurrTarget;

                if (time < s.require.time) {
                    range = s.optional.moveCurve.Evaluate (time / s.require.time);
                    CurrTarget = v * range + beginPosition;
                } else {
                    range = 1;
                    CurrTarget = s.require.targetPosition;
                }




                Vector2 lastV = currP - lastP;
                Vector2 currV = CurrTarget - currP;
                v = currV - lastV;
                float max = s.optional.maxAccelerate * Time.fixedDeltaTime;
                Vector2 nextP;
                if (v.magnitude >= max) {
                    currV = lastV + v * max / v.magnitude;
                    nextP = currP + currV;
                } else {
                    nextP = CurrTarget;
                }


                gameObject.Set2dPosition (nextP);

                currP = gameObject.Get2dPosition ();
                lastPosition = gameObject.Get2dPosition ();

                s.events.onMoving.Invoke ();
                if (currP == s.require.targetPosition) {
                    arrived = true;
                    s.events.onArrived.Invoke ();
                }

            }
        }

        private void OnEnable () {
            timebegin = Time.time;
            beginPosition = transform.position;
            arrived = false;
            setting.events.onStart.Invoke ();
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