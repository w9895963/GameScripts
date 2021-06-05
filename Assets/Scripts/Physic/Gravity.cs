using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Gravity : MonoBehaviour
{
    public Main main = new Main();


    private void Awake()
    {
        main.Initial(gameObject);
        main.onAddForce += () =>
        {
            enabled = true;
        };
        main.onStopForce += () =>
        {
            enabled = false;
        };
    }

    private void OnEnable()
    {
        main.AddForce();
    }

    private void OnDisable()
    {
        main.StopForce();

    }



    [System.Serializable]
    public class Main
    {
        public GameObject gameObject;
        public Vector2 gravityForce = new Vector2(0, -80);
        public Action onAddForce;
        public Action onStopForce;

        public bool enabled = false;

        public void Initial(GameObject gameObject)
        {
            this.gameObject = gameObject;
            StateCondition();
        }

        private void StateCondition()
        {

            List<Type> exist = new List<Type>();
            List<Type> except = new List<Type>() {
                typeof(StateBundle.OnGround)
            };
            StateF.AddStateCondition<StateBundle.Fall>(gameObject, exist, except);
            StateF.AddStateAction<StateBundle.Fall>(gameObject, () => AddForce(), () => StopForce());
            StateF.SetState<StateBundle.Fall>(gameObject, true);
        }

        private void FixedUpdateAction()
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(gravityForce);
        }

        public void AddForce()
        {
            if (enabled)
            {
                return;
            }
            enabled = true;
            onAddForce?.Invoke();
            BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
        }

        public void StopForce()
        {
            if (!enabled)
            {
                return;
            }
            enabled = false;
            onStopForce?.Invoke();
            BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
        }
    }
}

