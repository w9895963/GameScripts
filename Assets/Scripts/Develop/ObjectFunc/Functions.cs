using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
using Global.Physic;
using Global.Attack;
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



        public class WalkFunc : IFunctionCreate, ILateCreate
        {

            [System.Serializable]
            public class Data
            {
                public List<Setting> settings = new List<Setting>()
                {
                    new Setting(),
                    new Setting(){
                        state = AllState.Attack,
                        maxSpeed =1,
                    }
                };
                [System.Serializable]
                public class Setting
                {
                    public AllState state = default;
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
            private Data data;
            private Data.Setting set;
            private Data.Varables vrs;
            private Rigidbody2D rigidbody;
            private FunctionManager fm;
            private StateFunc state;
            private GameObject gameObject;
            private Func<Vector2> FetchInputValue = () => Vector2.zero;

            #endregion
            // * ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                gameObject = functionManager.gameObject;
                data = fm.GetData<Data>(this) ?? new Data();
                set = data.settings.Find((x) => x.state == AllState.Default);
                vrs = data.variables;

            }
            public void LateCreate()
            {
                state = fm.GetFunction<StateFunc>();
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
                    RemoveInputAction();
                }


            }


            private void StateCheckSection()
            {
                if (state == null)
                {
                    return;
                }

                state.AddStateChangedAction(() =>
                {
                    List<Data.Setting> settings = data.settings;
                    bool hasBreak = false;
                    foreach (var setIns in settings)
                    {
                        if (state.HasAll(setIns.state))
                        {
                            set = setIns;
                            hasBreak = true;
                            break;
                        }
                    }
                    if (!hasBreak)
                    {
                        set = settings.Find((x) => x.state == AllState.Default) ?? set;
                    }


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
                StateFunc st = state;
                if (st == null)
                {
                    return;
                }

                System.Action stateChangeAction = () =>
                  {


                      bool canAnimate = st.HasNo(AllState.Attack);
                      if (canAnimate)
                      {
                          if (st.HasNo(AllState.WalkForceLeft, AllState.WalkForceRight))
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
                          if (st.HasAll(AllState.FaceLeft))
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
                    if (vrs.walkState > 0)
                    {
                        state.Add(AllState.WalkForceRight);
                    }
                    else
                    {
                        state.Remove(AllState.WalkForceLeft, AllState.WalkForceRight);
                    }



                    if (vrs.walkState < 0)
                    {
                        state.Add(AllState.WalkForceLeft);
                    }
                    else
                    {
                        state.Remove(AllState.WalkForceLeft, AllState.WalkForceRight);
                    }


                    if (vrs.walkState < 0)
                    {
                        state.Add(AllState.FaceLeft);
                        state.Remove(AllState.FaceRight);
                    }
                    else if (vrs.walkState > 0)
                    {
                        state.Add(AllState.FaceRight);
                        state.Remove(AllState.FaceLeft);
                    }
                }
            }

            // * ---------------------------------- Public

            public void RemoveInputAction()
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
            private StateFunc state;
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
                state = fm.GetFunction<StateFunc>();
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
                if (!state.HasAll(AllState.OnGround))
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





        public class HitableFunction : IFunctionCreate, ILateCreate
        {

            // * ---------------------------------- 
            public static void AddAllHitableList(GameObject gameObject)
            {
                allHitableObjects.Add(gameObject);

                if (OnAllHitableListAdd != null)
                { OnAllHitableListAdd.Invoke(gameObject); }
            }

            public static void RemoveAllHitableList(GameObject gameObject)
            {
                allHitableObjects.Remove(gameObject);
                if (OnAllHitableListRemove != null)
                { OnAllHitableListRemove.Invoke(gameObject); }
            }
            public static Action<GameObject> OnAllHitableListAdd;
            public static Action<GameObject> OnAllHitableListRemove;
            public static List<GameObject> AllHitableObjects { get => allHitableObjects; }

            private static List<GameObject> allHitableObjects = new List<GameObject>();
            // * ---------------------------------- 

            [System.Serializable]
            public class Data
            {
                public List<AttackType> AllowAttackType = new List<AttackType>();


            }


            private FunctionManager fm;
            private Data data;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;


            public void OnCreate(FunctionManager functionManager)
            {

                fm = functionManager;
                data = fm.GetData<Data>(this);
                gameObject = fm.gameObject;


            }
            public void LateCreate()
            {
                rigidbody = gameObject.GetComponent<Rigidbody2D>();
                AttackTriggerSection();
                AddAllHitableList(gameObject);
                UnityEvent_OnDestroy.AddEvent(gameObject, () =>
                {
                    RemoveAllHitableList(gameObject);
                });

            }


            private void AttackTriggerSection()
            {
                UnityEvent_TriggerEnter2d.AddEvent(gameObject, (d) =>
                {
                    GameObject attackInsObj = d.gameObject;
                    AttackManagerCM attackInstanceCom = attackInsObj.GetComponent<AttackManagerCM>();
                });
            }
            private Component GetCollider()
            {
                return gameObject.GetComponentInChildren<Collider2D>();
            }




            public bool IsHitable(AttackType attackType)
            {
                return data.AllowAttackType.Contains(attackType);
            }



        }



        public class AttackFunc : IFunctionCreate, ILateCreate
        {

            [System.Serializable]
            public class Data
            {
                public AttackType attackType = default;

            }

            #region Privat Fields
            private FunctionManager fm;
            private Data data;
            private StateFunc state;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;
            private AttackProfile attackProfile;
            private bool reverseFacing = false;
            #endregion
            // * Region Privat Fields End---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                state = fm.GetFunction<StateFunc>();
                data = fm.GetData<Data>(this);
                gameObject = fm.gameObject;

            }
            public void LateCreate()
            {
                rigidbody = ReferenceFunction.GetComponent<Rigidbody2D>(gameObject);
            }




            // * ---------------------------------- 

            private void AttackAction()
            {
                GameObject attackManagerObj;
                AttackProfile profile = attackProfile;
                ResouceDynimicLoader.LoadAsync<GameObject>(profile.prefabPath, (loadObj) =>
                {
                    attackManagerObj = GameObject.Instantiate(loadObj);
                    FunctionManager.GetFunction<AttackManagerCM.AttackBuildFunc>(attackManagerObj)?.BuildAttack(gameObject, profile);
                });

            }



            // * ---------------------------------- 

            public void Attack()
            {
                attackProfile = AttackProfile.GetProfile(data.attackType);
                Animation.Play(gameObject, Animation.StateName.Attack);



                #region Set and Remove State
                if (state != null)
                {
                    state.Add(AllState.Attack);
                    reverseFacing = state.HasAll(AllState.FaceLeft);
                }
                else
                {
                    reverseFacing = false;
                }
                Timer.Wait(gameObject, attackProfile.delayTime + attackProfile.actionLastTime, () =>
                {
                    state.Remove(AllState.Attack);
                });
                #endregion
                // * Region Set and Remove State End---------------------------------- 




                Timer.Wait(gameObject, attackProfile.delayTime, AttackAction);

            }




        }

        public class PlayerAttackFunc : IFunctionCreate, ILateCreate
        {
            [System.Serializable]
            public class Data
            {


            }


            #region Basic Fields ------------
            private FunctionManager fm;
            private AttackFunc attackFunc;
            private StateFunc state;
            private GameObject gameObject;
            private Data data;
            #endregion
            // ** ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                this.fm = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;


                data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;

            }


            public void LateCreate()
            {
                InputManager.GetInputAction(InputManager.InputName.Shot).performed += ShotInputAction;
                attackFunc = fm.GetFunction<AttackFunc>();
                state = fm.GetFunction<StateFunc>();
            }

            private void ShotInputAction(InputAction.CallbackContext d)
            {
                bool stateAllowAttack = state.HasNo(AllState.Attack);
                if (stateAllowAttack)
                {
                    #region Play Animation
                    Animator animator = gameObject.GetComponentInChildren<Animator>();
                    RuntimeAnimatorController contrl = animator.runtimeAnimatorController;
                    bool exist = contrl.animationClips.First((clip) => clip.name == "Attack") != null;
                    if (exist)
                    {
                        Animation.Play(gameObject, Animation.StateName.Attack);

                    }
                    #endregion
                    // * Region Play Animation End---------------------------------- 



                    #region State Update
                    state.Add(AllState.Attack);
                    Timer.Wait(gameObject, 0.5f, () => { state.Remove(AllState.Attack); });
                    #endregion
                    // * Region State Update End---------------------------------- 


                    AttackUtility.CharacterAttack(gameObject, AttackType.HeroDefaultSlap);


                }
            }

        }
        public class AutoAttackFunc : IFunctionCreate, ILateCreate
        {
            [System.Serializable]
            public class Data
            {

            }


            #region Basic Fields ------------
            private FunctionManager fm;
            private AttackFunc attackFunc;
            private GameObject gameObject;
            private Data data;
            #endregion
            // ** ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                this.fm = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;

                attackFunc = fm.GetFunction<AttackFunc>();

                data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;

            }


            public void LateCreate()
            {
                void ShotOnTime()
                {
                    Timer.Wait(gameObject, 0.5f, () =>
                    {
                        AttackFunc attack = fm.GetFunction<AttackFunc>();
                        if (attack != null)
                        {
                            attack.Attack();
                            ShotOnTime();
                        }
                    });
                }
                ShotOnTime();
            }

        }


        public static class ExFunction
        {






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
                string stateName = state.ToString();
                if (animator == null)
                { return; }
                AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
                bool animationExist = animationClips.Any((clip) => clip.name == stateName);
                if (animationExist)
                {
                    animator.Play(stateName);
                }
            }

        }

        public enum AllState
        {
            Default,
            OnGround,
            WalkForceLeft,
            WalkForceRight,
            FaceLeft,
            FaceRight,
            Attack,
        }


    }

}
