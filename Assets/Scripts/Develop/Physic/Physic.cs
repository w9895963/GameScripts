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

            private List<AddForceCs> addForces = new List<AddForceCs> ();

            public Vector2 GetTotalForceAdd () {
                Vector2 result = Vector2.zero;
                addForces.ForEach ((x) => result += x.force);
                return result;
            }
            public Vector2 GetForceAdd (int index) {
                Vector2 result = Vector2.zero;
                addForces.ForEach ((x) => result += x.force);
                AddForceCs addForce = addForces.Find ((x) => x.index == index);
                return addForce != null?addForce.force : Vector2.zero;
            }
            public void SetForceAdd (int index, Vector2 force) {
                AddForceCs forceItem = addForces.Find ((x) => x.index == index);
                if (forceItem == null) {
                    forceItem = new AddForceCs ();
                    forceItem.index = currentIndex;
                    addForces.Add (forceItem);
                }
                forceItem.force = force;

            }


            private class AddForceCs {
                public int index;
                public Vector2 force;

            }
        }

        public enum PresetForceType {
            Gravity = 0
        }

      
    }
}