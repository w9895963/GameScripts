using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
using System;
using Global;
using Global.AttackBundle;
using CharacterBundle;

public class AttackManagerCM : MonoBehaviour, IFunctionManager
{


    public AttackBuildFunc.Data attackData = new AttackBuildFunc.Data();



    private FunctionManager fm;
    public FunctionManager Manager => fm;



    private void Awake()
    {
        fm = new FunctionManager(gameObject);
        fm.CreateFunction<AttackBuildFunc>(attackData);


        fm.CallLateCreateAction();
    }





    public class AttackBuildFunc : IFunctionCreate, ILateCreate

    {
        [System.Serializable]
        public class Data
        {
            public GameObject attacker;
            public AttackType attackType;
            public AttackProfile attackProfile;

        }


        #region Basic Fields ------------
        private FunctionManager functionManager;
        private GameObject gameObject;
        private Data data;
        #endregion
        // ** ---------------------------------- 

        private AttackType attackType;


        public void OnCreate(FunctionManager functionManager)
        {
            this.functionManager = functionManager;
            var fm = functionManager;
            gameObject = fm.gameObject;

            data = fm.GetData<Data>(this);
            data = data == null ? new Data() : data;

        }


        public void LateCreate()
        {

        }
        private bool IsShotAttack()
        {
            Bullet par = gameObject.GetComponent<Bullet>();
            return par != null;
        }
        private void BuildShot()
        {
            Bullet bullet = gameObject.GetComponent<Bullet>();
            ParticleSystem ptcS = gameObject.GetComponent<ParticleSystem>();
            AttackProfile profile = data.attackProfile;
            GameObject attacker = data.attacker;
            StateFunc state = FunctionManager.GetFunction<StateFunc>(attacker);

            //*Set Position
            Vector3 p = default;
            Quaternion r = default;
            AttackLocatorLo blpt = attacker.GetComponentInChildren<AttackLocatorLo>();
            if (blpt != null)
            {
                p = blpt.transform.position;
                r = blpt.transform.rotation;
            }
            else
            {
                p = attacker.transform.position;
                r = attacker.transform.rotation;
            }

            ptcS.gameObject.transform.SetPositionAndRotation(p, r);
            //* set rotate

            if (state != null)
            {
                bool faceLeft = state.HasAll(CharacterState.FaceLeft);
                ParticleSystem.ShapeModule shape = ptcS.shape;
                Vector3 ro = shape.rotation;
                if (faceLeft)
                {
                    ro.y = 180;
                    shape.rotation = ro;
                }
                else
                {
                    ro.y = 0;
                    shape.rotation = ro;
                }
            }
            //*Set Triger Objects
            List<GameObject> allHitableObjects = HitableFunction.AllHitableObjects;

            allHitableObjects.ForEach((obj) =>
            {
                HitableFunction hitable = FunctionManager.GetFunction<HitableFunction>(obj);
                if (hitable != null)
                {
                    if (hitable.IsHitable(profile.attackType))
                    {
                        Collider2D col = obj.GetComponentInChildren<Collider2D>();
                        if (col != null)
                            ptcS.trigger.AddCollider(col);
                    }
                }

            });

            Action<GameObject> action = (obj) =>
            {
                HitableFunction hit = FunctionManager.GetFunction<HitableFunction>(obj);
                bool canHIt = hit.IsHitable(attackType);
                if (canHIt)
                {
                    ptcS.trigger.AddCollider(obj.GetComponentInChildren<Collider2D>());
                }
            };
            HitableFunction.OnAllHitableListAdd += action;
            UnityEvent_OnDestroy.AddEvent(gameObject, () =>
            {
                HitableFunction.OnAllHitableListAdd -= action;
            });


            #region Reflact Shot

            #endregion
            // * Region Reflact Shot End---------------------------------- 
            bullet.AddParticleTriggerEnterAction((d) =>
            {
                List<Component> cs = d.GetColiders();
                List<Component> res = cs.FindAll((x) =>
                {
                    AttackManagerCM attackInstance = x.GetComponentInParent<AttackManagerCM>();
                    return attackInstance != null;
                });
                d.SetParticle((pr) =>
                   {
                       pr.velocity = -pr.velocity;
                       return pr;
                   }, res);



            });



            bullet.Shot();
        }
        private void BuildSlap()
        {
            AttackProfile profile = data.attackProfile;
            GameObject attacker = data.attacker;
            StateFunc state = FunctionManager.GetFunction<StateFunc>(attacker);

            #region Set Position and parent and Scale.x
            Transform transform = gameObject.transform;
            AttackLocatorLo attackPointer = attacker.GetComponentInChildren<AttackLocatorLo>();

            Transform transTar;
            if (attackPointer != null)
            {
                transTar = attackPointer.transform;
            }
            else
            {
                transTar = gameObject.transform;
            }

            transform.position = transTar.position;

            if (profile.setParent)
            {
                if (attackPointer != null)
                {
                    transform.parent = attackPointer.transform.parent;
                }
                else
                {
                    transform.parent = gameObject.transform;
                }

                Vector3 localScale = transform.localScale;
                float x_abs = localScale.x.Abs();
                localScale.x = x_abs;
                transform.localScale = localScale;
            }
            else
            {
                Vector3 localScale = transform.localScale;
                float x_abs = localScale.x.Abs();
                bool reverseFacing = false;
                if (state != null)
                    reverseFacing = state.HasAll(CharacterState.FaceLeft);
                localScale.x = reverseFacing ? x_abs * -1 : x_abs;
                transform.localScale = localScale;
            }


            #endregion
            // * Region Set Position and Scale.x End---------------------------------- 



            TimerMgr.Wait(gameObject, profile.slapObjectExistTime, () => gameObject.Destroy());
        }

        public void BuildAttack(GameObject attackerObj, AttackProfile profile)
        {
            data.attacker = attackerObj;
            data.attackProfile = profile;

            bool IsShot = IsShotAttack();

            if (IsShot)
            {
                BuildShot();
            }
            else
            {
                BuildSlap();
            }
        }

        public AttackProfile AttackProfile => data.attackProfile;

    }

}
