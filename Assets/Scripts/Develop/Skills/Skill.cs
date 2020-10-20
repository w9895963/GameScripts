using System;
using System.Collections;
using System.Collections.Generic;
using Global.Physic;
using static Global.Physic.PhysicUtility;
using UnityEngine;
using UnityEngine.Events;

namespace Global {
    namespace Skill {
        public static class SkillUtility {


        }


        public class GravityReverse {
            public GameObject target;
            public float lastTime;

            public GravityReverse (GameObject target, float lastTime, UnityAction endAction = null) {
                this.target = target;
                this.lastTime = lastTime;
                AddPhysicAction (target, PhysicOrder.GravityReverse, Action);

                Timer.WaitToCall (lastTime, () => {
                    if (endAction != null) {
                        endAction ();
                    }
                    RemovePhysicAction (target, Action);

                });
            }

            private void Action (ActionData data) {
                int index = PhysicOrder.Gravity;
                Vector2 gravityF = data.GetForce (index);
                data.SetForce (-gravityF * 2);
            }


        }
        
    }
}