using System;
using System.Collections;
using System.Collections.Generic;
using Global.Physic;
using static Global.Physic.PhysicUtility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Global {
    namespace Skill {
        public static class SkillUtility {


        }



        public class GravityReverse {
            public float lastTime;
            public TempObject temps = new TempObject ();

            private void PhysicAction (PhysicAction data) {
                int index = PhysicOrder.Gravity;
                Vector2 gravityF = data.GetForce (index);
                data.SetForce (-gravityF * 2);
            }


            public void Enable () {
                var gravityComps = GameObject.FindObjectsOfType<PhysicGravity> ();
                gravityComps.ForEach ((comp) => {
                    temps.AddEventTrigger = InputUtility.AddPointerEvent (comp, EventTriggerType.PointerClick, (d) => {
                        Cast (comp.gameObject);
                    });
                });
            }
            public void Disable () {
                temps.DestroyAll ();
            }


            public void Cast (GameObject target) {
                AddPhysicAction (target, PhysicOrder.GravityReverse, PhysicAction);

                Timer.WaitToCall (lastTime, () => {
                    RemovePhysicAction (target, PhysicAction);
                });
            }

        }

    }
}