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
        public enum AttackBasicType
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
            public AttackBasicType attackBasictype = AttackBasicType.Slap;


            public float delayTime = 0.2f;

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


            public static void CreateAttack(AttackType type,
             Vector2 position = default, float angle = 0, bool flipX = false)
            {
                AttackProfile attackProfile = AttackProfile.GetGlobalProfile(type);
                string path = attackProfile.prefabPath;

                ResouceDynimicLoader.LoadAsync<GameObject>(path, LoadAction);

                void LoadAction(GameObject attackPrefab)
                {
                    GameObject attackObject = CreateAttackObject(attackPrefab);
                    attackObject.SetPosition(position);
                    attackObject.SetRotation(angle);

                    SetFlipX(attackObject, flipX);

                }
                static GameObject CreateAttackObject(GameObject attack)
                {
                    GameObject obj = GameObject.Instantiate(attack);
                    return obj;
                }
                static void SetFlipX(GameObject obj, bool flipX)
                {
                    Vector3 localScale = obj.transform.localScale;
                    float x = localScale.x;
                    localScale.x = x.Abs() * (flipX == true ? -1 : 1);
                    obj.transform.localScale = localScale;
                }
            }

        }

    }
}
