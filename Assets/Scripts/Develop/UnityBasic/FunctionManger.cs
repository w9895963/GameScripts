using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.FunctionManager;
using Global.Physic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Global.Animate;
using System.Linq;

namespace Global
{
    namespace FunctionManager
    {


        public interface IFunctionManager
        {
            FunctionManager Manager { get; }
        }
        public interface IFunctionManagerInitial
        {
            void Initial(FunctionManager functionManager);
        }



        public class FunctionManager
        {
            private List<Function> functionList = new List<Function>();
            private List<Variable> variableList = new List<Variable>();
            public GameObject gameObject;

            public FunctionManager(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            public class Function
            {
                public System.Object function;
                public System.Type type;


            }
            public class Variable
            {
                public System.Object variable;
                public System.Type type;
            }


            public void AddFunction<T>(System.Object function)
            {
                Function f = new Function();
                f.function = function;
                f.type = typeof(T);
                functionList.Add(f);
            }
            public T GetFunction<T>()
            {
                Function f = functionList.Find((x) => x.type == typeof(T));
                return (T)f.function;
            }

            public void AddData<T>(System.Object variable)
            {
                Variable f = new Variable();
                f.variable = variable;
                f.type = typeof(T);
                variableList.Add(f);
            }
            public T GetData<T>()
            {
                Variable variable = variableList.Find((x) => x.type == typeof(T));
                return (T)variable.variable;
            }

            public void FastBuildUp<T, Y>(Y y) where T : new() where Y : class
            {
                T t = new T();
                AddFunction<T>(t);
                AddData<Y>(y);
                if (t is IFunctionManagerInitial)
                {
                    IFunctionManagerInitial t1 = (IFunctionManagerInitial)t;
                    t1.Initial(this);
                }
            }
        }



        public class GravityFunction
        {

            public class InitialData
            {
                public Rigidbody2D rigidbody;
                public Data variables;

            }

            [System.Serializable]
            public class Data
            {
                public bool enabled = false;

                public Vector2 gravity = new Vector2(0, -30);

            }


            private InitialData initialData = new InitialData();
            private Data variables;
            private SustainForce sustainForce;

            public GravityFunction(UnityAction<InitialData> initialAction)
            {
                InitialData ind = initialData;

                initialAction(ind);

                variables = ind.variables;


                sustainForce = ind.rigidbody.AddForceSustain(variables.gravity, ForceCalc);

            }
            public GravityFunction()
            {
            }
            public void Initial(FunctionManager functionManager)
            {
                FunctionManager fm = functionManager;
                variables = fm.GetData<Data>();
                Rigidbody2D rigidbody = fm.gameObject.GetComponent<Rigidbody2D>();
                sustainForce = rigidbody.AddForceSustain(variables.gravity, ForceCalc);
            }


            public void ForceCalc()
            {
                Data vrs = variables;
                Vector2 gravity = vrs.gravity * vrs.enabled.ToFloat();
                sustainForce.SetForce(gravity);

            }



            public void Disable()
            {
                variables.enabled = false;
            }
            public void Enable()
            {
                variables.enabled = true;
            }


        }

        public class WalkingFuncion
        {


            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public bool enabled = true;
                    public float moveForce = 50;
                    public float stopForce = 50;
                    public float maxSpeed = 10;
                    public GameObject walkAnimation;
                    public GameObject standAnimation;
                }

                public Varables variables = new Varables();
                [System.Serializable]
                public class Varables
                {
                    public float walkStat;
                    public bool canWalk = true;
                    public float buttonValue;
                    public float facing = 1;
                }
            }



            private Data.Setting set;
            private Data.Varables vrs;
            private GameObject gameObject;

            private VelosityChanger velosityChanger = new VelosityChanger();



            public void Initial(FunctionManager functionManager)
            {

                var dat = functionManager.GetData<Data>();
                gameObject = functionManager.gameObject;
                set = dat.setting;
                vrs = dat.variables;

                InputAction();
            }

            private void InputAction()
            {

                InputManager.GetInputAction(InputManager.InputName.Move).performed += (d) =>
               {
                   vrs.buttonValue = d.ReadValue<Vector2>().x;

                   Main();


               };
            }

            private void Main()
            {
                if (!set.enabled)
                {
                    return;
                }

                var butn = vrs.buttonValue;
                float walkStat = vrs.walkStat;
                bool canWalk = vrs.canWalk;


                if (butn != 0 & butn != walkStat & canWalk)
                {
                    Walk(butn);
                }
                else
                {
                    Stop();
                }
            }

            public void Walk(float direction)
            {
                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();

                vrs.facing = direction.Sign();
                float facing = vrs.facing;

                Vector2 targetSpeed = new Vector2(set.maxSpeed * direction.Sign(), 0);
                velosityChanger.StopFunction();
                velosityChanger = rigidbody.VelosityChangeTo(targetSpeed, set.moveForce, Vector2.right);




                AnimateUtility.ChangeAnimation(gameObject, set.walkAnimation, facing);

            }



            public void Stop()
            {
                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                float force = set.moveForce * 1;
                float facing = vrs.facing;

                Vector2 targetSpeed = new Vector2(0, 0);
                velosityChanger.StopFunction();
                velosityChanger = rigidbody.VelosityChangeTo(targetSpeed, force, Vector2.right);


                AnimateUtility.ChangeAnimation(gameObject, set.standAnimation, facing);


            }


        }


        public class GroundTestFunction
        {
            [System.Serializable]
            public class Data
            {
                public bool onGround = false;
                public List<ContactObject> contactList;
            }
            private FunctionManager functionManager;
            private Data data;
            private List<ContactObject> contactList = new List<ContactObject>();


            public void Initial(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                data = functionManager.GetData<Data>();
                data.contactList = contactList;

                UnityEventPort.AddCollisionAction(functionManager.gameObject, 0,
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
            }

            public bool IsOnGround()
            {
                return data.onGround;
            }



            [System.Serializable]
            public class ContactObject
            {
                public GameObject gameObject;
                public Vector2 normal;

            }


        }

        public class JumpFuntion
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

            }

            private FunctionManager fm;
            private SustainForce jump = null;
            private float beginJumpTime;


            public void Initial(FunctionManager functionManager)
            {
                fm = functionManager;
                InputAction inputAction = InputManager.GetInputAction(InputManager.InputName.Jump);
                inputAction.performed += ButtonAction;
            }

            private void ButtonAction(InputAction.CallbackContext data)
            {
                Data.Setting vrs = fm.GetData<Data>().setting;
                GameObject gameObject = fm.gameObject;
                Rigidbody2D rigidbody = fm.gameObject.GetComponent<Rigidbody2D>();


                float buttonState = data.ReadValue<float>();
                if (buttonState == 1)
                {
                    if (vrs.enabled == false)
                    { return; }

                    if (IsReayToJump())
                    {
                        jump = Jump(1);
                        beginJumpTime = Time.time;
                    }

                }
                else
                {
                    if (jump != null)
                    {
                        if (Time.time - beginJumpTime < vrs.minDuration)
                        {
                            Timer.BasicTimer(beginJumpTime + vrs.minDuration - Time.time, () =>
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

            public bool IsReayToJump()
            {
                GroundTestFunction groundTestFunction = fm.GetFunction<GroundTestFunction>();

                if (jump != null)
                { return false; }

                if (groundTestFunction != null)
                {
                    if (!groundTestFunction.IsOnGround())
                    { return false; }
                }


                return true;
            }

            public SustainForce Jump(float timeRate)
            {
                Data.Setting vrs = fm.GetData<Data>().setting;
                Rigidbody2D rigidbody = fm.gameObject.GetComponent<Rigidbody2D>();
                float time = timeRate.Map(0, 1, vrs.minDuration, vrs.maxDuration);

                jump = rigidbody.AddForceSustain(vrs.force * Vector2.up);
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

        public class HitableFunction : IFunctionManagerInitial
        {


            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public List<AttackFunction.AttackerClass> receiveAttack = new List<AttackFunction.AttackerClass>();
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {

                }

            }

            private FunctionManager fm;
            private Data.Setting set;
            private Data.Variables vrs;
            private GameObject gameObject;

            public void Initial(FunctionManager functionManager)
            {

                fm = functionManager;
                set = fm.GetData<Data>().setting;
                vrs = fm.GetData<Data>().variables;
                gameObject = fm.gameObject;


                AttackTriggerAction();
            }


            private void AttackTriggerAction()
            {
                UnityEventPort.AddTriggerAction(gameObject, 0, (d) =>
               {
                   AttackFunction attackFunction;
                   var hitSuccess = IsHitSuccess(out attackFunction);


                   if (hitSuccess)
                   {
                       HitAction();
                   }


                   bool IsHitSuccess(out AttackFunction attackFunction)
                   {
                       attackFunction = null;

                       IFunctionManager[] fms = d.colliderData.gameObject.GetComponentsInParent<IFunctionManager>();
                       if (fms.Length == 0) return false;

                       IFunctionManager fm = fms.ToList().Find((x) => x.Manager.GetFunction<AttackFunction>() != null);
                       if (fm == null) return false;

                       attackFunction = fm.Manager.GetFunction<AttackFunction>();
                       if (attackFunction == null) return false;

                       bool canHit = set.receiveAttack.Contains(attackFunction.GetAttackerClass());
                       return canHit;

                   }

                   void HitAction()
                   {
                       attackFunction.RecordHit(d.gameObject, gameObject);

                   }

               });
            }







        }

        public class AttackFunction : IFunctionManagerInitial
        {
            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                    public AttackerClass attackerClass;

                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {
                    public Dictionary<GameObject, List<GameObject>> hitNotes = new Dictionary<GameObject, List<GameObject>>();

                }

            }

            private Data.Setting set;
            private Data.Variables vrs;

            public void Initial(FunctionManager functionManager)
            {
                Data data = functionManager.GetData<AttackFunction.Data>();
                set = data.setting;
                vrs = data.variables;
            }

            public AttackerClass GetAttackerClass()
            {
                return set.attackerClass;
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
                Debug.Log(hitNotes.Keys.Count);
            }

            public enum AttackerClass
            {
                Hero, Enemy
            }

        }


    }

}
