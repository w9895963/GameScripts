using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Global.Physic.PhysicUtility;

namespace Global {
    namespace Physic {
        public static class PhysicUtility {
            public static Vector2 VelosityToForce (Vector2 velosityChanged, float mass = 1, bool isFixedUpdata = true) {
                var deltaTime = isFixedUpdata?Time.fixedDeltaTime : Time.deltaTime;
                return velosityChanged / deltaTime * mass;
            }
            public static void AddPhysicAction (GameObject gameObject, int runOrder, UnityAction<PhysicAction> action) {
                PhysicManager comp = gameObject.GetComponent<PhysicManager> ();
                if (comp != null) {
                    var actionList = comp.ActionList;
                    actionList.Add ((runOrder, action));
                    actionList.Sort ((x1, x2) => {
                        if (x1.Item1 < 0 & x2.Item1 >= 0) {
                            return 1;
                        } else {
                            return x1.Item1.CompareTo (x1.Item1);
                        }
                    });
                }
            }
            public static void RemovePhysicAction (GameObject gameObject, UnityAction<PhysicAction> action) {
                PhysicManager comp = gameObject.GetComponent<PhysicManager> ();
                if (comp != null) {
                    comp.ActionList.RemoveAll ((x) => x.Item2 == action);
                }
            }

            public static CollisionEvent AddColliderAction (GameObject gameObject, UnityAction<Collision2D> onEnter = null,
                UnityAction<Collision2D> onStay = null, UnityAction<Collision2D> onExit = null) {
                CollisionEvent evt = new CollisionEvent ();
                evt.gameObject = gameObject;
                evt.onEnter = onEnter;
                evt.onStay = onStay;
                evt.onExit = onExit;
                evt.Applay ();
                return evt;
            }


        }

        public class PIDcontrol {
            public Basic basic = new Basic ();
            public class Basic {
                public float deltaRate = 0.3f;
                public float changedRate = 45f;
            }
            public Optional optional = new Optional ();
            public class Optional {
                public bool enableMax = false;
                public float maximum = 60;
            }
            private bool initial = false;
            private Vector2 integrate = default;
            private Vector2 lastError;

            public PIDcontrol () { }
            public PIDcontrol (float deltaRate = 0.3f, float changedRate = 45f) {
                basic.deltaRate = deltaRate;
                basic.changedRate = changedRate;
            }


            public Vector2 CalcOutput (Vector2 error) {
                var s = basic;
                if (!initial) {
                    lastError = error;
                    initial = true;
                }
                Vector2 output;
                Vector2 delta = error - lastError;
                Vector2 wantDel = -error * s.deltaRate;
                Vector2 valueAdd = (wantDel - delta) * s.changedRate;
                integrate += -valueAdd;
                if (optional.enableMax) integrate = integrate.ClampMax (optional.maximum);

                lastError = error;
                output = integrate;

                float index = Time.time * 20;

                return output;
            }
        }

        public class JumpForce {
            public GameObject gameObject;
            public float jumpForce;
            public bool jumped = false;
            public JumpForce (GameObject gameObject, UnityAction<JumpForce> onCreate) {
                this.gameObject = gameObject;
                onCreate (this);
                AddPhysicAction (gameObject, PhysicOrder.Jump, Action);
            }
            public void Action (PhysicAction action) {
                if (!jumped) {
                    jumped = true;
                    action.SetForce (jumpForce * Vector2.up);
                    CollisionEvent evt = null;
                    evt = AddColliderAction (gameObject, (other) => {
                        bool hitGround = other.contacts.Any ((x) => x.normal.Angle (Vector2.up) < 60);
                        if (hitGround) {
                            evt.RemoveEvent ();
                        }
                    });
                }

            }
        }
        public class PhysicAction {
            private int currentIndex;
            public int CurrentIndex { get => currentIndex; set => currentIndex = value; }

            private List<PhysicData> forces = new List<PhysicData> ();

            public Vector2 GetTotalForce () {
                Vector2 result = Vector2.zero;
                forces.ForEach ((x) => result += x.force);
                return result;
            }
            public Vector2 GetForce (int index) {
                Vector2 result = Vector2.zero;
                forces.ForEach ((x) => result += x.force);
                PhysicData addForce = forces.Find ((x) => x.index == index);
                return addForce != null?addForce.force : Vector2.zero;
            }
            public void SetForce (Vector2 force) {
                PhysicData forceItem = forces.Find ((x) => x.index == currentIndex);
                if (forceItem == null) {
                    forceItem = new PhysicData ();
                    forceItem.index = currentIndex;
                    forces.Add (forceItem);
                }
                forceItem.force = force;

            }


            public PhysicData GetForceData (int index) {
                return forces.Find ((x) => x.index == index);

            }

            public class PhysicData {
                public int index;
                public Vector2 force;

            }
        }

        public class CollisionEvent {
            public GameObject gameObject;
            public UnityAction<Collision2D> onEnter;
            public UnityAction<Collision2D> onStay;
            public UnityAction<Collision2D> onExit;

            public void Applay () {
                PhysicEventHandler comp = gameObject.GetComponent<PhysicEventHandler> ();
                if (comp == null) {
                    comp = gameObject.AddComponent<PhysicEventHandler> ();
                }
                if (onEnter != null) {
                    comp.onCollisionEnter2D.AddListener (onEnter);
                    comp.eventCount++;
                }
                if (onStay != null) {
                    comp.onCollisionStay2D.AddListener (onStay);
                    comp.eventCount++;
                }
                if (onExit != null) {
                    comp.onCollisionExit2D.AddListener (onExit);
                    comp.eventCount++;
                }
            }
            public void RemoveEvent () {
                PhysicEventHandler comp = gameObject.GetComponent<PhysicEventHandler> ();
                if (comp != null) {
                    if (onEnter != null) {
                        comp.onCollisionEnter2D.RemoveListener (onEnter);
                        comp.eventCount--;
                    }
                    if (onStay != null) {
                        comp.onCollisionStay2D.RemoveListener (onStay);
                        comp.eventCount--;
                    }
                    if (onExit != null) {
                        comp.onCollisionExit2D.RemoveListener (onExit);
                        comp.eventCount--;
                    }
                    if (comp.eventCount == 0) {
                        GameObject.Destroy (comp);
                    }
                }
            }

        }

        public static class PhysicOrder {
            public static int Gravity = 0;
            public static int GravityReverse = 1;
            public static int Movement = 2;
            public static int Jump = 3;
        }




    }
}