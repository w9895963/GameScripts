using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Gravity gravity = new Gravity();

    [System.Serializable]
    public class Gravity
    {
        public bool enabled = true;
        public Vector2 force = new Vector2(0, -100);

    }
    public WalkingFuncion.Variables movement = new WalkingFuncion.Variables();



    private FixedUpdateAction fixedUpdateAction;
    private PhysicForceManager physicForceManager;

    private void Awake()
    {
        fixedUpdateAction = new FixedUpdateAction();

        physicForceManager = new PhysicForceManager((data) =>
        {
            data.rigidbody = gameObject.GetComponent<Rigidbody2D>();
            data.fixedUpdateAction = fixedUpdateAction;

        });



        WalkingFuncion walkingFunction = new WalkingFuncion((d) =>
        {
            d.inputAction = InputUtility.MoveInput;
            d.physicForceManager = physicForceManager;

        });


        if (movement.enabled)
        {
            walkingFunction.Enable();
        }




        #region Gravity

        GravityFunction gravityFunction = new GravityFunction((data) =>
        {
            data.physicForceManager = physicForceManager;
            data.gravity = gravity.force;
        });


        if (gravity.enabled)
        {
            gravityFunction.Enable();
        }

        #endregion



    }

    private void FixedUpdate()
    {
        fixedUpdateAction.RunAll();
    }




    public class FixedUpdateAction
    {
        private List<System.Action> actiontList = new List<System.Action>();
        public void Add(System.Action action)
        {
            actiontList.Add(action);
        }
        public void Remove(System.Action action)
        {
            actiontList.Remove(action);
        }
        public void RunAll()
        {
            actiontList.ForEach((ac) =>
            {
                ac();
            });
        }
    }
    public class PhysicForceManager
    {

        private InitialData initialData = new InitialData();
        public class InitialData
        {
            public Rigidbody2D rigidbody;
            public FixedUpdateAction fixedUpdateAction;
        }


        private Rigidbody2D rigidbody;
        private FixedUpdateAction fixedUpdateAction;
        public List<Force> forceList = new List<Force>();

        public PhysicForceManager(System.Action<InitialData> initialAction)
        {
            initialAction(initialData);
            rigidbody = initialData.rigidbody;
            fixedUpdateAction = initialData.fixedUpdateAction;

            fixedUpdateAction.Add(ApplyForce);

        }

        public void ApplyForce()
        {
            forceList.ForEach((f) =>
            {
                rigidbody.AddForce(f.forceValue);
            });
        }

        public void SetForce(Vector2 value, ForceType type)
        {
            if (forceList.Exists((x) => x.forceType == type))
            {
                Force force = forceList.Find((x) => x.forceType == type);
                force.forceValue = value;

            }
            else
            {
                Force force = new Force();
                force.forceType = type;
                force.runOrder = forceOrder[type];
                force.forceValue = value;

                forceList.Add(force);
                forceList.Sort((x, y) => x.runOrder.CompareTo(y.runOrder));

            }

        }

        public void RemoveForce(ForceType type)
        {
            forceList.RemoveAll((x) => x.forceType == type);
        }




        public enum ForceType
        {
            Gravity,
            Movement
        }

        public Dictionary<ForceType, int> forceOrder = new Dictionary<ForceType, int>() {
             {  ForceType.Gravity, 0  },
             {  ForceType.Movement, 0  },

        };

        public class Force
        {
            public Vector2 forceValue;
            public int runOrder;
            public ForceType forceType;

        }
    }



    public class GravityFunction
    {

        public class InitialData
        {
            public PhysicForceManager physicForceManager;
            public Vector2 gravity;

        }
        private InitialData initialData;


        public GravityFunction(UnityAction<InitialData> initialAction)
        {
            initialData = new InitialData();
            initialAction(initialData);
            gravity = initialData.gravity;
            physicForceManager = initialData.physicForceManager;

        }




        private PhysicForceManager physicForceManager;
        private Vector2 gravity;


        public void Disable()
        {
            physicForceManager.RemoveForce(PhysicForceManager.ForceType.Gravity);
        }
        public void Enable()
        {
            physicForceManager.SetForce(gravity, PhysicForceManager.ForceType.Gravity);
        }

    }

    public class WalkingFuncion
    {
        public class InitialData
        {
            public InputAction inputAction;
            public PhysicForceManager physicForceManager;
            public Variables variables;
        }

        [System.Serializable]
        public class Variables
        {
            public bool enabled = false;
            public float moveForce = 50;
            public float maxSpeed = 10;
        }




        private bool enable => initialData.variables.enabled;
        private InputAction inputAction => initialData.inputAction;
        private PhysicForceManager physicForceManager => initialData.physicForceManager;
        private float moveForce => initialData.variables.moveForce;
        private float maxSpeed => initialData.variables.maxSpeed;


        public InitialData initialData;
        private float moveButton;




        public WalkingFuncion(System.Action<InitialData> initialAction)
        {
            initialData = new InitialData();
            initialAction(initialData);

        }


        private void WalkingAction(InputAction.CallbackContext d)
        {
            moveButton = d.ReadValue<Vector2>().x;
            Vector2 force = new Vector2(moveForce * moveButton, 0);
            physicForceManager.SetForce(force, PhysicForceManager.ForceType.Movement);
        }

        public void Enable()
        {
            inputAction.performed += WalkingAction;
        }
        public void Disable()
        {
            inputAction.performed -= WalkingAction;
        }
    }



}