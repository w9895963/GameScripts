using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.FunctionModule;
using Global.Physic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Global.Animate;
using System.Linq;
using System;

namespace Global
{
    namespace FunctionModule
    {

        #region //*Interface


        public interface IFunctionManager
        {
            FunctionManager Manager { get; }
        }
        public interface IAwake
        {
            void Awake(FunctionManager functionManager);
        }


        #endregion






        public class FunctionManager
        {
            private List<Function> functionList = new List<Function>();
            public GameObject gameObject;

            public FunctionManager(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            public class Function
            {
                public System.Type type;
                public System.Object function;
                public System.Object data;


            }



            public T GetFunction<T>()
            {
                Function f = functionList.Find((x) => x.type == typeof(T));
                return (T)f.function;
            }

            public T GetData<T>(System.Object function)
            {
                Function variable = functionList.Find((x) => x.function == function);
                return (T)variable.data;
            }


            public T CreateFunction<T>(System.Object data = null) where T : new()
            {
                T function = new T();

                Function f = new Function();
                f.function = function;
                f.type = typeof(T);
                f.data = data;
                functionList.Add(f);

                if (function is IAwake)
                {
                    IAwake ini = (IAwake)function;
                    ini.Awake(this);
                }



                return function;
            }
        }


        public class TemplateFunction : IAwake
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
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;

            public void Awake(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;
                Data data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;
                set = data.setting;
                vrs = data.variables;
                Initial();

            }

            private void Initial()
            {

            }
        }


        public class ShareActionFunction : IAwake
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
                    public List<ActionInstance> actionList = new List<ActionInstance>();
                }

            }
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;

            public void Awake(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;
                Data data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;
                set = data.setting;
                vrs = data.variables;
                Initial();

            }

            private void Initial()
            {

            }
            [System.Serializable]
            public class ActionInstance
            {
                public string name;
                public System.Action action;
            }

            public void AddAction(System.Enum name, System.Action action)
            {
                ActionInstance a = new ActionInstance();
                a.name = name.ToString();
                a.action = action;
                vrs.actionList.Add(a);
            }
            public bool RunAction(System.Enum name)
            {
                ActionInstance actionInstance = vrs.actionList.Find((x) => x.name == name.ToString());
                if (actionInstance != null)
                {
                    actionInstance.action();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public class ReferenceFunction : IAwake
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

            public void Awake(FunctionManager functionManager)
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
        }
        public class ShareDataFunction : IAwake
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

            public void Awake(FunctionManager functionManager)
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
                public string dataName;
                public System.Func<System.Object> dataMap;
            }
            public void AddDateMap(System.Enum name, System.Func<System.Object> dataMap)
            {
                DataInst a = new DataInst();
                a.dataName = name.ToString();
                a.dataMap = dataMap;
                vrs.dataList.Add(a);
            }
            public T GetData<T>(System.Enum name)
            {
                T data;
                DataInst dataInst = vrs.dataList.Find((x) => x.dataName == name.ToString());
                data = (T)dataInst.dataMap();
                return data;
            }

        }
        public class StateFunction : IAwake
        {
            [System.Serializable]
            public class Data
            {
                public List<State> stateList = new List<State>();
            }
            private Data data;
            private System.Action onChangeAction;
            private (string state, Action action) currChanged;


            public void Awake(FunctionManager functionManager)
            {
                data = functionManager.GetData<Data>(this);
                onChangeAction += null;

            }


            // * ---------------------------------- 

            public class OnAddActionClass
            {
                public System.Action action;
                public string state;

            }
            [System.Serializable]
            public class State
            {
                public string name;
                public System.Object data;

            }
            public enum Action { add, remove }
            // * ---------------------------------- 

            public System.Action OnChangeAction
            {
                get => onChangeAction;
                set => onChangeAction = value;
            }
            // * ---------------------------------- 


            public void Add(string st, bool keepOnly = true, System.Object stateData = null)
            {
                if (keepOnly)
                {
                    bool exist = data.stateList.Exists((x) => x.name == st);
                    if (exist)
                    {
                        return;
                    }
                }
                State sta = new State();
                sta.name = st;
                sta.data = stateData;
                data.stateList.Add(sta);
                currChanged = (st, Action.add);

                onChangeAction.Invoke();
            }
            public void Add(System.Enum st, bool keepOnly = true)
            {
                Add(st.ToString(), keepOnly);
            }
            public void Remove(string st, System.Object stateData = null)
            {
                State state = data.stateList.Find((x) => x.name == st);
                if (state != null)
                {
                    data.stateList.Remove(state);
                    currChanged = (st, Action.remove);
                    onChangeAction.Invoke();
                }

            }
            public void Remove(System.Enum st)
            {
                Remove(st.ToString());
            }
            public void Remove(params string[] st)
            {
                if (st.Length > 0)
                {
                    st.ToList().ForEach((x) =>
               {
                   Remove(x);
               });
                }

            }
            public void Remove(params System.Enum[] st)
            {
                if (st.Length > 0)
                {
                    string[] vs = st.ToList().Select((x) => x.ToString()).ToList().ToArray();
                    Remove(vs);
                }

            }

            public bool HasNo(params string[] sts)
            {
                if (sts == null)
                {
                    return true;
                }
                if (sts.Length == 0)
                {
                    return true;
                }

                List<string> stateList = data.stateList.Select((x) => x.name).ToList();
                List<string> list = stateList.FindAll((x) => sts.Contains(x));
                return list.Count == 0;
            }
            public bool HasAll(params string[] sts)
            {
                bool re = true;
                if (sts == null)
                {
                    return true;
                }
                if (sts.Length == 0)
                {
                    return true;
                }
                sts.ForEach((m) =>
                {
                    bool v = data.stateList.Exists((x) => x.name == m);
                    if (!v)
                    {
                        re = false;
                    }
                });
                return re;
            }
            public bool HasNo(params System.Enum[] sts)
            {
                string[] vs = sts.ToList().Select((x) => x.ToString()).ToList().ToArray();
                return HasNo(vs);
            }
            public bool HasAll(params System.Enum[] sts)
            {
                string[] vs = sts.ToList().Select((x) => x.ToString()).ToList().ToArray();
                return HasAll(vs);
            }



        }




        public class InputFunction : IAwake
        {
            public enum State { InputLeft, InputRight, InputUp, InputDown }

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

                }

            }

            private FunctionManager fm;
            private Data.Setting set;
            private StateFunction state;
            public void Awake(FunctionManager functionManager)
            {
                fm = functionManager;
                set = fm.GetData<Data>(this).setting;
                state = fm.GetFunction<StateFunction>();
                InputManager.GetInputAction(InputManager.InputName.Move).performed += MoveInputAction;
            }

            private void MoveInputAction(InputAction.CallbackContext d)
            {
                if (set.moveInput == false) return;
                MoveFunction moveFuncion = fm.GetFunction<MoveFunction>();
                if (moveFuncion == null) return;
                Vector2 v = d.ReadValue<Vector2>();
                // moveFuncion.SetInput(v);
                if (v.x > 0)
                {
                    state.Remove(State.InputLeft);
                    state.Add(State.InputRight);
                }
                else if (v.x < 0)
                {
                    state.Remove(State.InputRight);
                    state.Add(State.InputLeft);
                }
                else
                {
                    state.Remove(State.InputRight, State.InputLeft);
                }
                if (v.y > 0)
                {
                    state.Remove(State.InputDown);
                    state.Add(State.InputUp);
                }
                else if (v.y < 0)
                {
                    state.Remove(State.InputUp);
                    state.Add(State.InputDown);
                }
                else
                {
                    state.Remove(State.InputUp, State.InputDown);
                }


            }

        }


        public class GroundTestFunction : IAwake
        {
            public enum State { onGround }
            [System.Serializable]
            public class Data
            {
                public bool onGround = false;
                public List<ContactObject> contactList = new List<ContactObject>();
            }
            private Data data;
            private GameObject gameObject;
            private StateFunction state;
            private FunctionManager fm;


            public void Awake(FunctionManager functionManager)
            {
                fm = functionManager;
                data = fm.GetData<Data>(this);
                gameObject = fm.gameObject;
                state = fm.GetFunction<StateFunction>();
                UnityEventPort.AddCollisionAction(gameObject, 0,
                                                 onCollisionEnter: OnCollisionEnter,
                                                 onCollisionExit: OnCollisionExit);
            }




            private void OnCollisionEnter(UnityEventPort.CallbackData data)
            {
                Collision2D other = data.collisionData;
                ContactObject contactObj = new ContactObject();
                contactObj.gameObject = other.gameObject;
                Vector2 normal = other.contacts[0].normal;
                contactObj.normal = normal;
                this.data.contactList.Add(contactObj);

                OnGroundTest();

            }

            private void OnCollisionExit(UnityEventPort.CallbackData data)
            {
                Collision2D other = data.collisionData;
                this.data.contactList.RemoveAll((m) => m.gameObject == other.gameObject);

                OnGroundTest();
            }

            private void OnGroundTest()
            {
                data.onGround = data.contactList.Exists((x) => x.normal.Angle(Vector2.up) < 5);
                if (data.onGround)
                {
                    if (state != null)
                    {
                        state.Add(State.onGround);
                    }

                }
                else
                {
                    if (state != null)
                    {
                        state.Remove(State.onGround);
                    }
                }
            }





            [System.Serializable]
            public class ContactObject
            {
                public GameObject gameObject;
                public Vector2 normal;

            }


        }

        public class GravityFunction : IAwake
        {
            [System.Serializable]
            public class Data
            {
                public Setting setting;
                [System.Serializable]
                public class Setting
                {
                    public bool enabled = true;

                    public Vector2 gravity = new Vector2(0, -100);

                }


            }

            private FunctionManager fm;
            private Data data;
            private Data.Setting set;
            private GameObject gameObject;
            private Physic.ConstantForce gravityForceObject;

            public void Awake(FunctionManager functionManager)
            {
                fm = functionManager;
                data = fm.GetData<Data>(this);
                set = data.setting;
                gameObject = fm.gameObject;

                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                gravityForceObject = rigidbody.AddConstantForce(set.gravity, ForceCalc);
            }



            private void ForceCalc()
            {
                Vector2 gravity = set.gravity * set.enabled.ToFloat();
                gravityForceObject.Force = gravity;

            }



            public void Disable()
            {
                set.enabled = false;
            }
            public void Enable()
            {
                set.enabled = true;
            }


        }

        public class MoveFunction : IAwake
        {
            public enum State { WalkForce, WalkForceFaceBack }


            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public float moveForce = 50;
                    public float maxSpeed = 10;
                    public float onAttackSpeed = 1;
                }

                public Varables variables = new Varables();
                [System.Serializable]
                public class Varables
                {
                    //controlstate
                    public bool onattack = false;
                    public float walkState = 0;
                    //force calc
                    public Vector2 targetV;
                    public Vector2 projectDir;
                    public Vector2 currV;
                    //animation
                    public float facing = 1;
                }
            }



            private Data.Setting set;
            private Data.Varables vrs;
            private Rigidbody2D rigidbody;
            private FunctionManager fm;
            private StateFunction stateFunction;
            private GameObject gameObject;

            // * ---------------------------------- 
            public void Awake(FunctionManager functionManager)
            {
                fm = functionManager;
                stateFunction = fm.GetFunction<StateFunction>();
                gameObject = functionManager.gameObject;
                var data = fm.GetData<Data>(this);
                set = data.setting;
                vrs = data.variables;
                rigidbody = gameObject.GetComponent<Rigidbody2D>();


                UnityEventPort.AddFixedUpdateAction(gameObject, 0, FixedUpdateAction);
                StateCondition();
            }

            // * ---------------------------------- 



            private void FixedUpdateAction(UnityEventPort.CallbackData d)
            {
                float mass = rigidbody.mass;


                vrs.currV = rigidbody.velocity;
                vrs.projectDir = Vector2.right;
                float maxSpeed = vrs.onattack ? set.onAttackSpeed : set.maxSpeed;
                vrs.targetV = new Vector2(maxSpeed * vrs.walkState, 0);

                Vector2 force = CalcForce(vrs.currV, vrs.projectDir, vrs.targetV, set.moveForce, mass);

                rigidbody.AddForce(force);

            }
            private void StateCondition()
            {
                StateFunction st = stateFunction;
                if (st == null)
                {
                    return;
                }

                st.OnChangeAction += () =>
                 {
                     //walk
                     if (st.HasAll(InputFunction.State.InputRight))
                     {
                         Walk(1);
                     }
                     if (st.HasAll(InputFunction.State.InputLeft))
                     {
                         Walk(-1);
                     }
                     if (st.HasNo(InputFunction.State.InputLeft, InputFunction.State.InputRight))
                     {
                         Walk(0);
                     }



                     bool canAnimate = st.HasNo(AttackFunction.AttackState);
                     if (canAnimate)
                     {
                         if (st.HasNo(State.WalkForce))
                         {
                             CommomMethod.PlayAnimatioin(gameObject, "Standing");
                         }
                         else
                         {
                             CommomMethod.PlayAnimatioin(gameObject, "Walking");
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


                     bool attacking = st.HasAll(AttackFunction.AttackState);
                     if (attacking)
                     {
                         vrs.onattack = true;
                     }
                     else
                     {
                         vrs.onattack = false;
                     }




                 };
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
            private void UpdateState()
            {
                //state
                if (stateFunction != null)
                {
                    if (vrs.walkState != 0)
                    {
                        stateFunction.Add(State.WalkForce);
                    }
                    else
                    {
                        stateFunction.Remove(State.WalkForce);
                    }


                    if (vrs.walkState < 0)
                    {
                        stateFunction.Add(State.WalkForceFaceBack);
                    }
                    else if (vrs.walkState > 0)
                    {
                        stateFunction.Remove(State.WalkForceFaceBack);
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
        public class JumpFuntion : IAwake
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


            public void Awake(FunctionManager functionManager)
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





        public class HitableFunction : IAwake
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

            public void Awake(FunctionManager functionManager)
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

        public class AttackFunction : IAwake
        {
            public const string AttackState = "Attack";

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


            public void Awake(FunctionManager functionManager)
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
                        if (st.HasNo(AttackState))
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
                    stateFunction.Add(AttackState);
                    Timer.Wait(gameObject, set.time, () =>
                    {
                        stateFunction.Remove(AttackState);
                    });
                }

                CommomMethod.PlayAnimatioin(gameObject, set.animationName);
            }


        }

        public class ShotFunc : IAwake
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

            public void Awake(FunctionManager functionManager)
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

                state.OnChangeAction += () =>
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


        public static class CommomMethod
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



        }


    }

}
