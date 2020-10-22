using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Global.Physic.PhysicUtility;

namespace Global {
    namespace Physic {
        public static class PhysicUtility {
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

            public static void AddColliderEvent (UnityAction<PhysicAction> action) { }



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

        public static class PhysicOrder {
            public static int Gravity = 0;
            public static int GravityReverse = 1;
            public static int Movement = 2;
            public static int Jump = 3;
        }


    }
}