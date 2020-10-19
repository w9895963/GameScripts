using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Global {
    namespace Skill {
        public static class SkillTool {
            private static SkillManager skillManager;
            private static List<SkillObj> skills;


            public static void Cast (SkillObj.SkillProfile skillProfile) {
                throw new NotImplementedException ();
            }

            public static SkillManager SkillManager {
                get {
                    if (skillManager == null) {
                        skillManager = GameObject.FindObjectOfType<SkillManager> ();
                    }
                    return skillManager;
                }
            }

          



        }
        public class SkillInstance {
            public string skillName;
            public SkillType skillType;
            public ISkillTarget target;
            public enum SkillType { ReverseGravity }



            public void Cast () {
                switch (skillType) {
                    case SkillType.ReverseGravity:

                    default:
                        break;
                }
            }

        }

        public enum SkillType { ReverseGravity }
    }

}