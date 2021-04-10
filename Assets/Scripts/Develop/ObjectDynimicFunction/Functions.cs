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
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public Rigidbody2D rigidbody;
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {

                }

            }
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;

            public void OnCreate(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;
                Data data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                Initial();

            }

            private void Initial()
            {
                if (set.rigidbody == null)
                {
                    set.rigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
                }
                ShareDataFunction shareDataFunction = functionManager.GetFunction<ShareDataFunction>();
                if (shareDataFunction != null)
                {
                    shareDataFunction.AddDateMap(ShareData.Rigidbody2D, () => set.rigidbody);
                }
            }

            public static Rigidbody2D GetRigidbody2D(GameObject gameObject)
            {
                FunctionManager fm = FunctionManager.GetFunctionManager(gameObject);
                if (fm != null)
                {
                    ReferenceFunction refe = fm.GetFunction<ReferenceFunction>();
                    if (refe != null)
                    {
                        return refe.set.rigidbody;
                    }
                }

                return gameObject.GetComponentInChildren<Rigidbody2D>();
            }
        }
        public class InputFunction : IFunctionCreate
        {
            public enum State { InputLeft, InputRight, InputUp, InputDown, MoveInput }
            public enum ShareData { MoveInput }

            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool moveInput = true;
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public Vector2 moveInputValue = Vector2.zero;
                }

            }

            private FunctionManager fm;
            private Data.Setting set;
            private Data.Variables vrs;
            private StateFunction state;
            private ShareDataFunction sdata;

            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                Data data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                state = fm.GetFunction<StateFunction>();
                sdata = fm.GetFunction<ShareDataFunction>();
                InputManager.GetInputAction(InputManager.InputName.Move).performed += MoveInputAction;
                sdata.AddDateMap(ShareData.MoveInput, () => vrs.moveInputValue);

            }

            private void MoveInputAction(InputAction.CallbackContext d)
            {
                Vector2 pre = vrs.moveInputValue;
                Vector2 curr = d.ReadValue<Vector2>();


                bool currin = curr.magnitude != 0;
                bool preIn = pre.magnitude != 0;
                bool stateChange = currin != preIn;

                if (stateChange)
                {
                    if (currin)
                    {
                        state.Add(State.MoveInput, () => vrs.moveInputValue);
                    }
                    else
                    {
                        state.Remove(State.MoveInput);
                    }
                }

                vrs.moveInputValue = curr;
                sdata.CallDataUpdateAction(ShareData.MoveInput);

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
                    // public string W
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

            #region Basic Fields  ------------------
            private Data.Setting set;
            private Data.Varables vrs;
            private Rigidbody2D rigidbody;
            private FunctionManager fm;
            private StateFunction state;
            private ShareDataFunction shareData;
            private GameObject gameObject;
            private Func<Vector2> FetchInputValue = () => Vector2.zero;

            #endregion
            // * ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                state = fm.GetFunction<StateFunction>();
                shareData = fm.GetFunction<ShareDataFunction>();
                gameObject = functionManager.gameObject;
                var data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;

            }
            public void LateCreate()
            {
                rigidbody = ExFunction.GetRigidbody2D(fm);
                InputSection();
                StateCheckSection();
                UnityEventPort.AddFixedUpdateAction(gameObject, 0, FixedUpdateSection);
                AnimationConditionSection();
            }



            private void InputSection()
            {
                if (state == null)
                {
                    return;
                }
                if (shareData == null)
                { return; }

                const InputFunction.ShareData md = InputFunction.ShareData.MoveInput;
                shareData.AddDataUpdateAction(md, () =>
                {
                    vrs.moveInput = shareData.GetData<Vector2>(md);
                    inputStateCheck();
                });

                void inputStateCheck()
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


            private void FixedUpdateSection(UnityEventPort.CallbackData d)
            {

                float mass = rigidbody.mass;


                vrs.currV = rigidbody.velocity;
                vrs.projectDir = Vector2.right;
                float maxSpeed = vrs.onattack ? set.onAttackSpeed : set.maxSpeed;
                vrs.targetV = new Vector2(maxSpeed * vrs.walkState, 0);

                Vector2 force = CalcForce(vrs.currV, vrs.projectDir, vrs.targetV, set.moveForce, mass);

                rigidbody.AddForce(force);

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
                              ExFunction.PlayAnimatioin(gameObject, "Standing");
                          }
                          else
                          {
                              ExFunction.PlayAnimatioin(gameObject, "Walking");
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








            // * ---------------------------------- Action


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
        public class JumpFuntion : IFunctionCreate
        {

            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool enabled = true;
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
                state = fm.GetFunction<StateFunction>();
                Data data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                gameObject = fm.gameObject;
                rigidbody = fm.gameObject.GetComponent<Rigidbody2D>();


                InputAction inputAction = InputManager.GetInputAction(InputManager.InputName.Jump);
                inputAction.performed += InputAction;
            }

            private void InputAction(InputAction.CallbackContext data)
            {

                float buttonState = data.ReadValue<float>();
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

            public bool JumpConditionTest()
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





        public class HitableFunction : IFunctionCreate
        {


            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public List<AttackFunction.AttackerType> receiveAttack = new List<AttackFunction.AttackerType>();
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public GameObject attackInstacne;
                    public GameObject attacker;
                    internal bool hitSuccess;
                }

            }

            private FunctionManager fm;
            private Data.Setting set;
            private Data.Variables vrs;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;

            public void OnCreate(FunctionManager functionManager)
            {

                fm = functionManager;
                set = fm.GetData<Data>(this).setting;
                vrs = fm.GetData<Data>(this).variables;
                gameObject = fm.gameObject;
                rigidbody = gameObject.GetComponent<Rigidbody2D>();



                AttackTriggerAction();
            }


            private void AttackTriggerAction()
            {
                UnityEventPort.AddTriggerAction(gameObject, 0, (d) =>
               {
                   vrs.hitSuccess = false;
                   AttackFunction attackFunction;
                   GameObject attackInsObj = d.colliderData.gameObject;
                   AttackInstance attackInstanceCom = attackInsObj.GetComponent<AttackInstance>();

                   vrs.hitSuccess = IsHitSuccess(out attackFunction);
                   if (vrs.hitSuccess)
                   {
                       vrs.attackInstacne = attackInsObj;
                       vrs.attacker = attackInstanceCom.data.attacker;
                       HitAction();
                   }


                   bool IsHitSuccess(out AttackFunction attackFunction)
                   {
                       attackFunction = null;
                       if (attackInstanceCom == null) return false;


                       return true;

                   }



               });


            }


            public void HitAction()
            {
                Vector2 vct = gameObject.GetPosition2d() - vrs.attackInstacne.GetPosition2d();
                rigidbody.AddForce(vct.Project(Vector2.right).normalized * 200f);

            }




        }

        public class AttackFunction : IFunctionCreate
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
                    public AttackerType attackerType = AttackerType.Hero;
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
                stateFunction = functionManager.GetFunction<StateFunction>();
                gameObject = functionManager.gameObject;
                set = dat.setting;
                vrs = dat.variables;

                InputAction inputAction = InputManager.GetInputAction(InputManager.InputName.Attack);
                inputAction.performed += AttackInputAction;
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


            public AttackerType GetAttackerClass()
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

            public enum AttackerType
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

                ExFunction.PlayAnimatioin(gameObject, set.animationName);
            }


        }

        public class ShotFunc : IFunctionCreate
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

                }

            }
            private FunctionManager fm;
            private StateFunction state;
            private GameObject gameObject;
            private Rigidbody2D rigidbody;

            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                state = fm.GetFunction<StateFunction>();
                gameObject = fm.gameObject;
                rigidbody = gameObject.GetComponent<Rigidbody2D>();



                InputManager.GetInputAction(InputManager.InputName.Shot).performed += ShotInputAction;
                StateCondition();

            }

            private void StateCondition()
            {

                if (state == null) return;

                System.Action onChangeAction = () =>
                {
                    ParticleSystem pt = gameObject.GetComponentInChildren<ParticleSystem>();
                    ParticleSystem.ShapeModule shape = pt.shape;
                    Vector3 r = shape.rotation;
                    if (state.HasAll(MoveFunction.State.WalkForceFaceBack))
                    {
                        r.y = 180;
                        shape.rotation = r;
                    }
                    else
                    {
                        r.y = 0;
                        shape.rotation = r;
                    }
                };
                state.AddStateChangedAction(onChangeAction);
            }


            private void ShotInputAction(InputAction.CallbackContext d)
            {
                if (rigidbody != null)
                {
                    Vector2 f = Vector2.right * 20;
                    if (!state.HasAll(MoveFunction.State.WalkForceFaceBack))
                    {
                        f.x *= -1;
                    }

                    rigidbody.AddForce(f, ForceMode2D.Impulse);
                }

                ParticleSystem pt = gameObject.GetComponentInChildren<ParticleSystem>();
                pt.Play();
            }


        }


        public static class ExFunction
        {
            public static void PlayAnimatioin(GameObject parentObject, string animationName)
            {
                GameObject gameObject = parentObject;
                AnimationHolder animationHolder = gameObject.GetComponentInChildren<AnimationHolder>();
                if (animationHolder == null) return;

                animationHolder.gameObject.GetComponentInChildren<Animation>().Play(animationName);


            }

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

            public static Rigidbody2D GetRigidbody2D(FunctionManager fm)
            {
                Rigidbody2D rs = null;
                ShareDataFunction r = fm.GetFunction<ShareDataFunction>();
                if (r != null)
                {
                    rs = r.GetData<Rigidbody2D>(ReferenceFunction.ShareData.Rigidbody2D);
                }
                if (rs == null)
                {
                    rs = fm.gameObject.GetComponentInChildren<Rigidbody2D>();
                }
                return rs;

            }



        }


    }

}
