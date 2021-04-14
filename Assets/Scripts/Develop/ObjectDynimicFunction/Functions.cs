using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
using Global.Physic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Global.Animate;
using System.Linq;
using System;

namespace Global
{
    namespace ObjectDynimicFunction
    {








        public class ShareDataFunction : IFunctionCreate
        {
            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public List<DataInst> dataList = new List<DataInst>();
                }



            }
            private FunctionManager functionManager;
            private FunctionManager fm;
            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;
            private List<(string dataname, System.Action action)> onDataUpdateActions = new List<(string dataname, Action action)>();

            public void OnCreate(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                this.fm = functionManager;
                gameObject = fm.gameObject;
                Data data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                Initial();

            }
            private void Initial()
            {

            }
            [System.Serializable]
            public class DataInst
            {
                public string name;
                public System.Func<System.Object> dataMap;
            }
            public void AddDateMap(System.Enum name, System.Func<System.Object> dataMap)
            {
                DataInst a = new DataInst();
                a.name = name.ToString();
                a.dataMap = dataMap;
                vrs.dataList.Add(a);
            }

            public T GetData<T>(System.Enum name)
            {
                T data;
                DataInst dataInst = vrs.dataList.Find((x) => x.name == name.ToString());
                if (dataInst == null)
                {
                    return default;
                }
                data = (T)dataInst.dataMap();
                return data;
            }
            public void CallDataUpdateAction(System.Enum dataName)
            {
                var ac = onDataUpdateActions.FindAll((x) => x.dataname == dataName.ToString());
                ac.ForEach((x) => x.action());


            }
            public void AddDataUpdateAction(System.Enum dataName, System.Action action)
            {
                (string dataname, Action action) a = (dataName.ToString(), action);
                onDataUpdateActions.Add(a);
            }
            public void RemoveDataUpdateAction(System.Action action)
            {
                onDataUpdateActions.RemoveAll((x) => x.action == action);
            }

        }




        public class ReferenceFunction : IFunctionCreate
        {
            public enum ShareData { Rigidbody2D }
            [System.Serializable]
            public class Data
            {
                public Rigidbody2D rigidbody;
                public Animator animator;

            }
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data data;

            public void OnCreate(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;
                data = fm.GetData<Data>(this);
                Initial();

            }

            private void Initial()
            {
                Rigidbody2D rigidbody = data.rigidbody;
                data.rigidbody = GetComponent<Rigidbody2D>(rigidbody);

                Animator animator = data.animator;
                data.animator = GetComponent<Animator>(animator);

            }

            private T GetComponent<T>(T t) where T : Component
            {
                T result = t;
                if (result == null)
                {
                    result = gameObject.GetComponentInChildren<T>();
                }
                return result;
            }

            public static T GetComponent<T>(GameObject gameObject) where T : Component
            {
                T result = null;
                ReferenceFunction reff = FunctionManager.GetFunction<ReferenceFunction>(gameObject);
                if (reff != null)
                {
                    if (IsType<Rigidbody2D>())
                    {
                        Component com = reff.data.rigidbody;
                        result = (T)com;
                    }
                    else if (IsType<Animator>())
                    {
                        Component com = reff.data.animator;
                        result = (T)com;
                    }
                }


                if (result == null)
                {
                    result = gameObject.GetComponentInChildren<T>();
                }


                return result;


                bool IsType<O>()
                {
                    return typeof(T).Equals(typeof(O));
                }
            }
        }



        public class MoveFunction : IFunctionCreate, ILateCreate
        {
            public enum State { WalkForce, WalkForceFaceBack }


            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool enableInputControl = true;
                    public float moveForce = 50;
                    public float maxSpeed = 10;
                    public float onAttackSpeed = 1;
                }

                public Varables variables = new Varables();
                [System.Serializable]
                public class Varables
                {
                    //state
                    public Vector2 moveInput;
                    public bool onattack = false;
                    public float walkState = 0;
                    //force calc
                    public Vector2 targetV;
                    public Vector2 projectDir;
                    public Vector2 currV;
                    //animation
                    public int facing = 1;
                }
            }

            #region  Fields  ------------------
            private Data.Setting set;
            private Data.Varables vrs;
            private Rigidbody2D rigidbody;
            private FunctionManager fm;
            private StateFunction state;
            private GameObject gameObject;
            private Func<Vector2> FetchInputValue = () => Vector2.zero;

            #endregion
            // * ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                gameObject = functionManager.gameObject;
                var data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;

            }
            public void LateCreate()
            {
                state = fm.GetFunction<StateFunction>();
                rigidbody = ReferenceFunction.GetComponent<Rigidbody2D>(gameObject);
                InputSection();
                StateCheckSection();
                FixedUpdateSection();
                AnimationConditionSection();
            }

            private void InputSection()
            {
                if (set.enableInputControl == true)
                {
                    AddInputAction();
                }
                else
                {
                    RemoveInputACtion();
                }


            }


            private void StateCheckSection()
            {
                if (state == null)
                {
                    return;
                }
                state.AddStateAction_Add(AttackFunction.State.Attack, () =>
                {
                    vrs.onattack = true;
                });
                state.AddStateAction_Remove(AttackFunction.State.Attack, () =>
                {
                    vrs.onattack = false;
                });

            }

            private void FixedUpdateSection()
            {
                UnityEventPort.AddFixedUpdateAction(gameObject, 0, (d) =>
                {
                    float mass = rigidbody.mass;


                    vrs.currV = rigidbody.velocity;
                    vrs.projectDir = Vector2.right;
                    float maxSpeed = vrs.onattack ? set.onAttackSpeed : set.maxSpeed;
                    vrs.targetV = new Vector2(maxSpeed * vrs.walkState, 0);

                    Vector2 force = CalcForce(vrs.currV, vrs.projectDir, vrs.targetV, set.moveForce, mass);

                    rigidbody.AddForce(force);
                });
            }
            // * ---------------------------------- 
            private void inputStateCheck()
            {
                float input = vrs.moveInput.x;
                if (input != 0)
                {
                    Walk(input);
                }
                else
                {
                    Stop();
                }
            }
            private void inputAction(InputAction.CallbackContext d)
            {
                vrs.moveInput = d.ReadValue<Vector2>();
                inputStateCheck();
            }
            private static Vector2 CalcForce(Vector2 currV, Vector2 direction, Vector2 targetVelosity,
                                                                 float maxForce = 1000f, float mass = 1)
            {
                if (direction != default)
                {
                    targetVelosity = targetVelosity.Project(direction);
                    currV = currV.Project(direction);
                }

                Vector2 deltaV = targetVelosity - currV;
                Vector2 forceTotalNeed = deltaV / Time.fixedDeltaTime * mass;
                Vector2 outForce = forceTotalNeed.ClampMax(maxForce);
                return outForce;
            }


            private void AnimationConditionSection()
            {
                StateFunction st = state;
                if (st == null)
                {
                    return;
                }

                System.Action stateChangeAction = () =>
                  {


                      bool canAnimate = st.HasNo(AttackFunction.State.Attack);
                      if (canAnimate)
                      {
                          if (st.HasNo(State.WalkForce))
                          {
                              Animation.Play(gameObject, Animation.StateName.Stand);
                          }
                          else
                          {
                              Animation.Play(gameObject, Animation.StateName.Walking);
                          }


                          // set faceing
                          var animationHolder = gameObject.GetComponentInChildren<AnimationHolder>();
                          if (animationHolder == null)
                          {
                              return;
                          }

                          Transform transform = animationHolder.gameObject.transform;
                          Vector3 localScale = transform.localScale;
                          if (st.HasAll(State.WalkForceFaceBack))
                          {
                              localScale.x = localScale.x.Abs() * -1;
                          }
                          else
                          {
                              localScale.x = localScale.x.Abs() * 1;
                          }
                          transform.localScale = localScale;

                      }


                  };
                st.AddStateChangedAction(stateChangeAction);
            }

            private void UpdateState()
            {
                //state
                if (state != null)
                {
                    if (vrs.walkState != 0)
                    {
                        state.Add(State.WalkForce);
                    }
                    else
                    {
                        state.Remove(State.WalkForce);
                    }


                    if (vrs.walkState < 0)
                    {
                        state.Add(State.WalkForceFaceBack);
                    }
                    else if (vrs.walkState > 0)
                    {
                        state.Remove(State.WalkForceFaceBack);
                    }
                }
            }

            // * ---------------------------------- Public

            public void RemoveInputACtion()
            {
                InputManager.GetInputAction(InputManager.InputName.Move).performed -= inputAction; ;
            }

            public void AddInputAction()
            {
                InputManager.GetInputAction(InputManager.InputName.Move).performed += inputAction; ;
            }


            public void Walk(float direction)
            {
                vrs.walkState = direction;
                UpdateState();

            }

            public void Stop()
            {
                vrs.walkState = 0;
                UpdateState();
            }


        }
        public class JumpFuntion : IFunctionCreate, ILateCreate
        {

            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool enabled = true;
                    public bool enableInput = true;
                    public float force = 200f;
                    public float maxDuration = 0.2f;
                    public float minDuration = 0.1f;

                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public float beginJumpTime;
                }

            }

            private FunctionManager fm;
            private StateFunction state;
            private Data.Setting set;
            private Data.Variables vrs;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;
            private Physic.ConstantForce jump = null;


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
            }
            public void LateCreate()
            {
                state = fm.GetFunction<StateFunction>();
                Data data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                gameObject = fm.gameObject;
                rigidbody = ReferenceFunction.GetComponent<Rigidbody2D>(gameObject);

                InputSection();


            }
            private void InputSection()
            {
                if (set.enableInput)
                {
                    AddInputAction();
                }
                else
                {
                    RemoveInputAction();
                }
            }

            private void JumpInputAction(InputAction.CallbackContext d)
            {
                float buttonState = d.ReadValue<float>();
                if (buttonState == 1)
                {
                    if (set.enabled == false)
                    { return; }

                    if (JumpConditionTest())
                    {
                        jump = Jump(1);
                        vrs.beginJumpTime = Time.time;
                    }

                }
                else
                {
                    if (jump != null)
                    {
                        if (Time.time - vrs.beginJumpTime < set.minDuration)
                        {
                            Timer.BasicTimer(vrs.beginJumpTime + set.minDuration - Time.time, () =>
                            {
                                if (jump == null) return;
                                jump.Disable();
                                jump = null;
                            });
                        }
                        else
                        {
                            jump.Disable();
                            jump = null;
                        }


                    }

                }
            }

            private bool JumpConditionTest()
            {
                if (state == null)
                {
                    return true;
                }
                if (!state.HasAll(GroundTestFunction.State.onGround))
                {
                    return false;
                }

                return true;
            }
            // * ---------------------------------- 
            public void RemoveInputAction()
            {
                throw new NotImplementedException();
            }

            public void AddInputAction()
            {
                InputManager.GetInputAction(InputManager.InputName.Jump).performed += JumpInputAction;
            }



            public Physic.ConstantForce Jump(float timeRate)
            {
                Data.Setting vrs = fm.GetData<Data>(this).setting;
                Rigidbody2D rigidbody = fm.gameObject.GetComponent<Rigidbody2D>();
                float time = timeRate.Map(0, 1, vrs.minDuration, vrs.maxDuration);

                jump = rigidbody.AddConstantForce(vrs.force * Vector2.up);
                Timer.BasicTimer(vrs.maxDuration, () =>
                {
                    if (jump == null)
                    {
                        return;
                    }
                    jump.Disable();
                    jump = null;
                });

                return jump;
            }


        }





        public class HitableFunction : IFunctionCreate, ILateCreate, IOnDestroy
        {

            public static List<GameObject> AllHitableObjects = new List<GameObject>();

            [System.Serializable]
            public class Data
            {
                public List<AttackFunc.AttackType> AllowAttackType = new List<AttackFunc.AttackType>();


                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public GameObject attackInstacne;
                }

            }


            private FunctionManager fm;
            private Data data;
            private Data.Variables vrs;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;

            public void OnCreate(FunctionManager functionManager)
            {

                fm = functionManager;
                data = fm.GetData<Data>(this);
                vrs = data.variables;
                gameObject = fm.gameObject;
                AllHitableObjects.Add(gameObject);

            }
            public void LateCreate()
            {
                rigidbody = gameObject.GetComponent<Rigidbody2D>();
                AttackTriggerSection();
            }
            public void OnDestroy()
            {
                AllHitableObjects.Remove(gameObject);
            }


            private void AttackTriggerSection()
            {
                UnityEventPort.AddTriggerAction(gameObject, 0, (UnityAction<UnityEventPort.CallbackData>)((d) =>
               {
                   GameObject attackInsObj = d.colliderData.gameObject;
                   AttackInstance attackInstanceCom = attackInsObj.GetComponent<AttackInstance>();




               }));


            }




            public bool IsHitable(AttackFunc.AttackType attackType)
            {
                return data.AllowAttackType.Contains(attackType);
            }
            public void HitAction()
            {
                Vector2 vct = gameObject.GetPosition2d() - vrs.attackInstacne.GetPosition2d();
                rigidbody.AddForce(vct.Project(Vector2.right).normalized * 200f);

            }


        }

        public class AttackFunction : IFunctionCreate, ILateCreate
        {
            public enum State { Attack }

            // * ---------------------------------- 
            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool enableInput = true;
                    public string animationName = "Attack";
                    public AttackType attackerType = AttackType.Hero;
                    public float time = 0.6f;
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public Dictionary<GameObject, List<GameObject>> hitNotes = new Dictionary<GameObject, List<GameObject>>();

                }

            }

            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;
            private FunctionManager fm;
            private Data dat;
            private StateFunction stateFunction;


            // * ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                dat = functionManager.GetData<AttackFunction.Data>(this);

                gameObject = functionManager.gameObject;
                set = dat.setting;
                vrs = dat.variables;

            }
            public void LateCreate()
            {
                stateFunction = fm.GetFunction<StateFunction>();
                // InputAction inputAction = InputManager.GetInputAction(InputManager.InputName.Attack);
                // inputAction.performed += AttackInputAction;  
            }

            // * ---------------------------------- 
            private void AttackInputAction(InputAction.CallbackContext d)
            {
                if (set.enableInput == false)
                {
                    return;
                }
                if (d.ReadValue<float>() == 1)
                {
                    bool canAttack = false;
                    var st = stateFunction;
                    if (st != null)
                    {
                        if (st.HasNo(State.Attack))
                        {
                            canAttack = true;
                        }
                    }

                    if (canAttack)
                    {
                        Attack();
                    }


                }
            }


            public AttackType GetAttackerClass()
            {
                return set.attackerType;
            }
            public void RecordHit(GameObject attackInstance, GameObject hitObject)
            {
                Dictionary<GameObject, List<GameObject>> hitNotes = vrs.hitNotes;
                bool v = hitNotes.Keys.Contains(attackInstance);
                if (!v)
                {
                    hitNotes.Add(attackInstance, new List<GameObject>());
                }

                hitNotes[attackInstance].Add(hitObject);
            }

            public enum AttackType
            {
                Hero,
                Enemy
            }
            // * ---------------------------------- 
            public bool EnableInput
            {
                set => set.enableInput = value;
                get => set.enableInput;
            }
            public void Attack()
            {
                StateFunction stateFunction = fm.GetFunction<StateFunction>();
                if (stateFunction != null)
                {
                    var attackState = State.Attack;
                    stateFunction.Add(attackState);
                    Timer.Wait(gameObject, set.time, () =>
                    {
                        stateFunction.Remove(attackState);
                    });
                }

                Animation.Play(gameObject, Animation.StateName.Attack);
            }


        }

        public class AttackFunc : IFunctionCreate, ILateCreate
        {
            public enum State { Attack }
            [System.Serializable]
            public class Data
            {
                public AttackType attackType = default;
                public List<AttackProfile> attackProfiles = AttackProfile.Default;

            }
            private FunctionManager fm;
            private Data data;
            private StateFunction state;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;

            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                state = fm.GetFunction<StateFunction>();
                data = fm.GetData<Data>(this);
                gameObject = fm.gameObject;

            }
            public void LateCreate()
            {
                rigidbody = ReferenceFunction.GetComponent<Rigidbody2D>(gameObject);
                InputManager.GetInputAction(InputManager.InputName.Shot).performed += ShotInputAction;
            }

            // * ---------------------------------- 

            private void ShotInputAction(InputAction.CallbackContext d)
            {
                Attack();
            }

            private void SetupParticleSystem(ParticleSystem ptcS)
            {
                //*Set Position
                Vector3 p = default;
                Quaternion r = default;
                AttackPointer blpt = gameObject.GetComponentInChildren<AttackPointer>();
                p = blpt.transform.position;
                r = blpt.transform.rotation;
                ptcS.gameObject.transform.SetPositionAndRotation(p, r);
                //* set rotate
                if (state != null)
                {
                    bool v = state.HasAll(MoveFunction.State.WalkForceFaceBack);
                    ParticleSystem.ShapeModule shape = ptcS.shape;
                    Vector3 ro = shape.rotation;
                    if (v)
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
                    if (hitable.IsHitable(data.attackType))
                    {
                        ptcS.trigger.AddCollider(obj.GetComponent<Collider2D>());
                    }
                });
            }
            private void ParticleTriggerEvent(Bullet.HitData hitData)
            {
                List<Component> colliders = hitData.GetColiders();
                colliders.ForEach((Action<Component>)((cld) =>
                  {
                      var hitableFunction = FunctionManager.GetFunction<HitableFunction>(cld.gameObject);
                      if (hitableFunction == null)
                      { return; }

                      var particles = hitData.GetParticles(cld);
                      particles.ForEach((p) =>
                      {

                      });

                  }));



                hitData.ParticleSystem.TriggerSubEmitter(0);



            }

            private void AttackAction()
            {
                GameObject attackObj;
                AttackProfile profile = AttackProfile.GetProfile(data.attackType, data.attackProfiles);
                ResouceDynimicLoader.LoadAsync<GameObject>(profile.prefabPath, (loadObj) =>
                {
                    attackObj = GameObject.Instantiate(loadObj);
                    AfterPrefabLoad();
                });

                void AfterPrefabLoad()
                {
                    ParticleSystem bulletSystem = attackObj.gameObject.GetComponent<ParticleSystem>();
                    if (bulletSystem != null)
                    {
                        Bullet bullet = bulletSystem.GetComponent<Bullet>();

                        SetupParticleSystem(bulletSystem);
                        bullet.AddParticleTriggerEnterAction(ParticleTriggerEvent);
                        bullet.Shot();
                    }
                    else
                    {

                        #region Set Position and Scale.x
                        Transform transform = attackObj.transform;
                        Transform transTar = gameObject.GetComponentInChildren<AttackPointer>().transform;
                        transform.position = transTar.position;
                        if (state != null)
                        {
                            Vector3 localScale = transform.localScale;
                            float x_abs = localScale.x.Abs();
                            bool reverse = state.HasAll(MoveFunction.State.WalkForceFaceBack);
                            localScale.x = reverse ? x_abs * -1 : x_abs;
                            transform.localScale = localScale;
                        }
                        #endregion
                        // * Region Set Position and Scale.x End---------------------------------- 

                        Timer.Wait(gameObject, profile.slapColliderExistTime, () => attackObj.Destroy());

                        if (state != null)
                        {
                            Timer.Wait(gameObject, profile.actionLastTime, () =>
                            {
                                state.Remove(State.Attack);
                            });
                        }


                        AttackInstance attackInstance = attackObj.GetComponentInChildren<AttackInstance>();
                        attackInstance.attacker = gameObject;
                        attackInstance.attackProfile = profile;




                    }
                }





            }
            // * ---------------------------------- 
            public class AttackProfile
            {
                public Animation.StateName playAnimation = Animation.StateName.Attack;
                public string prefabPath = "Prefab/DefaultHit";
                public AttackType shotType = default;
                public float delayTime = 0.4f;
                public Vector2 backFireForce = default;

                public float actionLastTime = 1f;

                public float slapColliderExistTime = 1f;

                public bool bulletPierce = false;


                public AttackProfile(AttackType defaultTypeSetup)
                {
                    if (defaultTypeSetup == AttackType.HeroDefaultSlap)
                    {

                    }
                    else if (defaultTypeSetup == AttackType.HeroDefaultShot)
                    {

                    }
                }
                public AttackProfile() => new AttackProfile(AttackType.HeroDefaultSlap);



                public static List<AttackProfile> Default = new List<AttackProfile>()
                {
                   new AttackProfile(AttackType.HeroDefaultSlap),
                   new AttackProfile(AttackType.HeroDefaultShot)
                };

                public static AttackProfile GetProfile(AttackType shotType, List<AttackProfile> profiles)
                {
                    AttackProfile result = null;

                    result = profiles.Find((x) => x.shotType == shotType);
                    if (result == null)
                    {
                        result = Default.Find((x) => x.shotType == shotType);
                    }
                    if (result == null)
                    {
                        result = new AttackProfile();
                    }

                    return result;
                }

            }

            public enum AttackType
            {
                HeroDefaultSlap,
                HeroDefaultShot,
            }


            // * ---------------------------------- 

            public void Attack()
            {
                AttackProfile attackProfile = AttackProfile.GetProfile(data.attackType, data.attackProfiles);
                Animation.Play(gameObject, attackProfile.playAnimation);
                if (state != null)
                {
                    state.Add(State.Attack);
                }

                Timer.Wait(gameObject, attackProfile.delayTime, AttackAction);

            }



        }


        public static class ExFunction
        {


            public static T GetFunctionInParent<T>(GameObject gameObject) where T : class
            {
                List<FunctionManager> functionManagers = gameObject.GetComponentsInParent<IFunctionManager>().ToList()
                .Select((x) => x.Manager).ToList();
                if (functionManagers.Count == 0) return null;
                FunctionManager functionManager = functionManagers.ToList().Find((x) => x.GetFunction<T>() != null);
                if (functionManager == null) return null;
                T t = functionManager.GetFunction<T>();
                return t;
            }



        }





        public static class ResouceDynimicLoader
        {
            public static List<LoadFile> loadFiles = new List<LoadFile>();

            public class LoadFile
            {
                public System.Object file;
                public string path;
            }


            public static void LoadAsync<T>(string path, Action<T> action) where T : UnityEngine.Object
            {
                LoadFile loadFile = loadFiles.Find((x) => x.path == path);
                if (loadFile != null)
                {
                    action((T)loadFile.file);
                }
                else
                {
                    ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
                    Action<AsyncOperation> actionA = (d) =>
                    {
                        UnityEngine.Object file = resourceRequest.asset;
                        LoadFile lf = new LoadFile();
                        lf.path = path;
                        lf.file = file;
                        loadFiles.Add(lf);
                        action((T)file);
                    };
                    resourceRequest.completed += actionA;
                }

            }

        }

        public class Animation
        {

            public static string paramaterName = "StateIndex";
            public enum StateName
            {
                Stand = 0,
                Attack = 3,
                Walking = 1,
                Shot = 2,
            }

            public static void Play(GameObject gameObject, StateName state)
            {
                Animator animator = gameObject.GetComponentInChildren<Animator>();

                animator.SetInteger(paramaterName, (int)state);
            }

        }


    }

}
