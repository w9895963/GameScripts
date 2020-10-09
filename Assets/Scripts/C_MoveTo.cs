    using System.Collections.Generic;
    using System.Collections;
    using Global;
    using UnityEngine.Events;
    using UnityEngine;

    public class C_MoveTo : MonoBehaviour {
        public Behaviour behaviour = default;
        public enum Behaviour { Simple, PID }

        [System.Serializable] public class SimpleMove {

            [System.Serializable] public class Require {
                public Vector2 targetPosition;
                public float time;
            }
            public Require require = new Require ();
            [System.Serializable] public class Optional {
                public AnimationCurve moveCurve = Global.Curve.ZeroOne;
            }
            public Optional optional = new Optional ();
            public Events events = new Events ();

            // * ---------------------------------- 
            [SerializeField] private Variables vars = new Variables ();
            [System.Serializable] public class Variables {
                public float timebegin;
                public Vector2 beginPosition;
                public Object createBy;
                public bool arrived = false;
                public Vector2 lastPosition;
            }
            private GameObject gameObject;

            // * ---------------------------------- 

            public void UpdatePosition () {
                float time = Time.time - vars.timebegin;
                Vector2 currP = gameObject.Get2dPosition ();
                Vector2 lastP = vars.lastPosition;
                Vector2 targetPosition = require.targetPosition;
                Vector2 v = targetPosition - vars.beginPosition;
                Vector2 normal = v.Rotate (90).normalized;
                float dist = v.magnitude;
                Vector2 beginPosition = vars.beginPosition;
                float range;
                Vector2 nextP;

                if (time < require.time) {
                    range = optional.moveCurve.Evaluate (time / require.time);
                    nextP = v * range + beginPosition;
                } else {
                    range = 1;
                    nextP = require.targetPosition;
                }


                gameObject.SetPosition (nextP);

                currP = gameObject.Get2dPosition ();
                vars.lastPosition = gameObject.Get2dPosition ();

                events.onMoving.Invoke ();
                if (currP == require.targetPosition) {
                    vars.arrived = true;
                    events.onArrived.Invoke ();
                }
            }


            public void Initial (GameObject gameObject, Events events) {
                this.gameObject = gameObject;
                vars.timebegin = Time.time;
                vars.beginPosition = gameObject.transform.position;
                vars.arrived = false;
                this.events = events;
                this.events.onStart.Invoke ();
            }
        }
        public SimpleMove simpleMove = new SimpleMove ();
        // * ---------------------------------- 


        public PIDMove pidMove = new PIDMove ();
        [System.Serializable] public class PIDMove {
            public Require require = new Require ();
            [System.Serializable] public class Require {
                public Vector2 targetPosition;
                public float speed = 4f;
            }
            public Advance advance = new Advance ();
            [System.Serializable] public class Advance {
                public float P = 0.5f;
                public float I = 0.1f;
                public float curveMaxdistance = 1;
                public AnimationCurve slowDownCurve = Curve.ZeroOneFastOut01;
                public float minVelosity = 0.1f;
            }
            public Variables vars = new Variables ();
            // * ---------------------------------- 
            [System.Serializable] public class Variables {
                public Vector2 compensateForce = Vector2.zero;
                public bool moving;
            }
            private GameObject gameObject;
            private Events events;


            // * ---------------------------------- 
            public void Initial (GameObject gameObject, Events events) {
                this.gameObject = gameObject;
                this.events = events;
                this.events.onStart.Invoke ();
            }

            public void updateForce () {
                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();
                if (rigidbody == null) rigidbody = gameObject.AddComponent<Rigidbody2D> ();
                float deltaTime = Time.fixedDeltaTime;
                Vector2 position = rigidbody.position;

                float rotation = rigidbody.rotation;
                float rotationStep = 360 * deltaTime;


                Vector2 targetPosition = require.targetPosition;
                float speed = require.speed;

                float p = advance.P;
                float I = advance.I;
                AnimationCurve slowDownCurve = advance.slowDownCurve;
                float distance = advance.curveMaxdistance;
                float minVelosity = advance.minVelosity;

                Vector2 fixedForce = vars.compensateForce;
                bool lastFrameMoving = vars.moving;


                bool moving;
                Vector2 forceAdd;


                #region //*Main Body

                Vector2 distVt = targetPosition - position;
                float dist = distVt.magnitude;
                Vector2 wantVlosity = slowDownCurve.Evaluate (dist, 0, distance, 0, speed) * distVt.normalized;
                wantVlosity = wantVlosity.normalized * (wantVlosity.magnitude + minVelosity);
                Vector2 velocity = rigidbody.velocity;
                float frameMove = velocity.magnitude * deltaTime;
                Vector2 error = wantVlosity - velocity;




                if (frameMove >= dist) {
                    moving = false;
                } else {
                    moving = true;
                }

                fixedForce += error * I * dist / deltaTime;
                forceAdd = error / deltaTime + fixedForce;
                rigidbody.AddForce (forceAdd);

                if (!moving) {
                    rigidbody.velocity = Vector2.zero;
                    rigidbody.position = targetPosition;
                }


                if (!lastFrameMoving & moving) {
                    events.onStart.Invoke ();
                }
                if (moving) {
                    events.onMoving.Invoke ();
                }
                if (lastFrameMoving & !moving) {
                    events.onArrived.Invoke ();
                }


                #endregion


                vars.compensateForce = fixedForce;
                vars.moving = moving;


            }
        }

        //*-----------------------
        public Events events = new Events ();
        [System.Serializable] public class Events {
            public UnityEvent onArrived = new UnityEvent ();
            public UnityEvent onMoving = new UnityEvent ();
            public UnityEvent onStart = new UnityEvent ();
        }
        //*------------------------
        [ReadOnly] public Object createBy;


        //*-----------------------------
        private void FixedUpdate () {

            if (behaviour == Behaviour.Simple) {
                simpleMove.UpdatePosition ();
            } else if (behaviour == Behaviour.PID) {
                pidMove.updateForce ();
            }

        }


        private void OnEnable () {
            simpleMove.Initial (gameObject, events);
            pidMove.Initial (gameObject, events);
        }


    }


    public static class Extension_C_MoveTo {

        public static C_MoveTo MoveTo (this GameObjectExMethod source, UnityAction<C_MoveTo.SimpleMove> setup) {
            C_MoveTo comp = source.gameObject.AddComponent<C_MoveTo> ();
            comp.enabled = false;

            C_MoveTo.SimpleMove setting = new C_MoveTo.SimpleMove ();
            setup (setting);
            comp.simpleMove = setting;
            comp.createBy = source.callby;

            comp.enabled = true;
            return comp;

        }

    }