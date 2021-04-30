using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Global.ObjectDynimicFunction;

namespace Global
{
    namespace Attack
    {
        public enum AttackType
        {
            HeroDefaultSlap,
            HeroDefaultShot,
        }


        [System.Serializable]
        public class AttackProfile
        {
            public static List<AttackProfile> AllProfiles = DefaultAllProfiles;
            // * ---------------------------------- 
            public string prefabPath = "Prefab/DefaultHit";
            public AttackType attackType = default;
            public float delayTime = 0.2f;
            public float attackBackForce = 0;
            public float hitBackForce = 0;
            public bool setParent = true;


            public float actionLastTime = 0.4f;

            public float slapObjectExistTime = 0.2f;

            public bool bulletPierce = false;

            public AttackProfile(AttackType defaultTypeSetup)
            {
                if (defaultTypeSetup == AttackType.HeroDefaultSlap)
                {

                }
                else if (defaultTypeSetup == AttackType.HeroDefaultShot)
                {
                    attackType = AttackType.HeroDefaultShot;
                    prefabPath = "Prefab/DefaultShot";
                }
            }
            public AttackProfile() => new AttackProfile(AttackType.HeroDefaultSlap);

            // * ---------------------------------- 

            public static List<AttackProfile> DefaultAllProfiles => new List<AttackProfile>()
                {
                   new AttackProfile(AttackType.HeroDefaultSlap),
                   new AttackProfile(AttackType.HeroDefaultShot)
                };




            public static AttackProfile GetProfile(AttackType type)
            {
                AttackProfile result = null;

                result = AllProfiles.Find((x) => x.attackType == type);
                if (result == null)
                {
                    result = new AttackProfile();
                }


                return result;
            }

        }
        public static class AttackUtility
        {
            public static void CharacterAttack(GameObject gameObject, AttackType type)
            {
                AttackProfile profile = AttackProfile.GetProfile(type);
                string prefabPath = profile.prefabPath;
                AttackType attackType = profile.attackType;

                AttackManagerCM cm = gameObject.GetComponentsInChildren<AttackManagerCM>().First((x) =>
                {
                    AttackManagerCM.AttackBuildFunc attackBuildFunc = FunctionManager.GetFunction<AttackManagerCM.AttackBuildFunc>(x.gameObject);
                    AttackType atype = attackBuildFunc.AttackProfile.attackType;
                    bool v = atype == type;
                    return true;
                });

                bool hasManager = cm != null;
                if (hasManager)
                {
                    cm.GetComponent<Animator>().Play("Attack", 0, 0);
                }


            }

        }

    }
}
