using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Gravity : MonoBehaviour
{
    public enum State { Gravity }
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
            ObjectState.OnStateAdd.Add(gameObject, GroundFinder.State.OnGround, () =>
            {
                StopForce();
            });
            ObjectState.OnStateRemove.Add(gameObject, GroundFinder.State.OnGround, () =>
            {
                AddForce();
            });
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
            ObjectState.State.Add(gameObject, State.Gravity);
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
            ObjectState.State.Remove(gameObject, State.Gravity);
        }
    }
}

