using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Global.Physic.PhysicUtility;

namespace Global {
    namespace Physic {
        public static class PhysicUtility {
            public static void AddPhysicAction (GameObject gameObject, int runOrder, UnityAction<ActionData> action) {
                PhysicForceAction comp = gameObject.GetComponent<PhysicForceAction> ();
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
            public static void RemovePhysicAction (GameObject gameObject, UnityAction<ActionData> action) {
                PhysicForceAction comp = gameObject.GetComponent<PhysicForceAction> ();
                if (comp != null) {
                    comp.ActionList.RemoveAll ((x) => x.Item2 == action);
                }
            }



        }
        public class ActionData {
            private int currentIndex;
            public int CurrentIndex { get => currentIndex; set => currentIndex = value; }

            private List<ForceData> forces = new List<ForceData> ();

            public Vector2 GetTotalForce () {
                Vector2 result = Vector2.zero;
                forces.ForEach ((x) => result += x.force);
                return result;
            }
            public Vector2 GetForce (int index) {
                Vector2 result = Vector2.zero;
                forces.ForEach ((x) => result += x.force);
                ForceData addForce = forces.Find ((x) => x.index == index);
                return addForce != null?addForce.force : Vector2.zero;
            }
            public void SetForce (Vector2 force) {
                ForceData forceItem = forces.Find ((x) => x.index == currentIndex);
                if (forceItem == null) {
                    forceItem = new ForceData ();
                    forceItem.index = currentIndex;
                    forces.Add (forceItem);
                }
                forceItem.force = force;

            }
            public ForceData GetForceData (int index) {
                return forces.Find ((x) => x.index == index);

            }



            public class ForceData {
                public int index;
                public Vector2 force;

            }
        }

        public static class PhysicOrder {
            public static int Gravity = 0;
            public static int GravityReverse = 1;
        }


    }
}