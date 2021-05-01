using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Global.ObjectDynimicFunction;
using Global.ResouceBundle;
using System;

namespace Global
{
    namespace AttackBundle
    {
        public enum AttackType
        {
            HeroDefaultSlap,
            HeroDefaultShot,
        }
        public enum AttackBasictype
        {
            Slap,
            Shot
        }


        [System.Serializable]
        public class AttackProfile
        {
            public static List<AttackProfile> AllProfiles = DefaultAllProfiles;
            // * ---------------------------------- 
            //Key
            public AttackType attackType = default;

            //PreBuild
            public string prefabPath = "Prefab/DefaultSlap";


            public AttackBasictype attackBasictype = AttackBasictype.Slap;

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




            public static AttackProfile GetGlobalProfile(AttackType type)
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


        public static class Attack
        {
            public static void CharacterAttack(GameObject gameObject, AttackType type)
            {
                PrepareAttackObject(gameObject, type,
                RunAttack);



                #region Local Method

                static void PrepareAttackObject(GameObject gameObject, AttackType type, Action<GameObject> callbackAction)
                {
                    GameObject attackObj = null;

                    AttackProfile profile = AttackProfile.GetGlobalProfile(type);
                    string prefabPath = profile.prefabPath;
                    AttackType attackType = type;

                    AttackManagerCM cm = gameObject.GetComponentsInChildren<AttackManagerCM>().ToList().Find((x) =>
                    {
                        AttackManagerCM.AttackBuildFunc attackBuildFunc = FunctionManager.GetFunction<AttackManagerCM.AttackBuildFunc>(x.gameObject);
                        AttackType atype = attackBuildFunc.AttackProfile.attackType;
                        return atype == type;
                    });

                    attackObj = cm?.gameObject;

                    if (attackObj != null)
                    {
                        callbackAction(attackObj);
                        return;
                    }

                    ResouceDynimicLoader.LoadAsync<GameObject>(prefabPath, (loadObj) =>
                    {

                        callbackAction(loadObj);
                    });

                }


                static void RunAttack(GameObject obj)
                {
                    Debug.Log(obj);
                    Slap slap = obj.GetComponent<Slap>();
                    if (slap != null)
                    {
                        slap.DoSlap();
                    }

                }
                #endregion
                // * Region Local Method End---------------------------------- 

            }



        }

    }
}
