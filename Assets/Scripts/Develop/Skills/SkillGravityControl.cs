using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using static Global.GlobalPropety;

namespace Global {
    namespace Skill {
        public static class GravityControlEx {
            public static void GravityControl (this SkillInstance skill, IManager target, Vector2 gravity) {
                IGravity com = target as IGravity;
                
                if (com != null)
                    com.Gravity = gravity;
            }
            public static void GravityReverse (this SkillInstance skill, IManager target) {
                IGravity gravity = target as IGravity;
                if (gravity != null)
                    gravity.Gravity = -gravity.Gravity;
            }

        }


    }

}