using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFriction : MonoBehaviour
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
        public enum State { GroundFriction }
        public float frictionForce = 100f;
        public Vector2 groundNormal = Vector2.up;
        public Vector2 groundNormalDefault = Vector2.up;
        public GameObject gameObject;
        public Action onAddForce;
        public Action onStopForce;

        public bool enabled = false;

        public Rigidbody2D body => gameObject.GetRigidbody2D();
        private float mass => body.mass;
        public Vector2 velocity => body.velocity;
        public Vector2 GroundNormal => groundNormal != Vector2.zero ? groundNormal : groundNormalDefault;
        public Vector2 groundDirection => GroundNormal.RotateTo(Vector2.right, 90).normalized;
        public float speed => velocity.ProjectToFloat(groundDirection);


        private void FixedUpdateAction()
        {
            Vector2 velocity = body.velocity;
            float force = MathPh.SpeedChangeForce(speed, 0, mass);
            force = force.ClampAbsMax(frictionForce);
            body.AddForce(groundDirection * force);
        }
        public void Initial(GameObject gameObject)
        {
            this.gameObject = gameObject;

            StateCondition();
            ConnectDate();
        }

        private void ConnectDate()
        {
            ObjectDate.OnDateUpdate(gameObject, GroundFinder.Date.GroundNormal, (d) =>
           {
               groundNormal = (Vector2)d;
           });
        }

        private void StateCondition()
        {
            ObjectState.OnStateAdd.Add(gameObject, GroundFinder.State.OnGround, () =>
            {
                AddForce();

            });
            ObjectState.OnStateRemove.Add(gameObject, () =>
            {
                AddForce();
            }, Physic.Move.State.MoveLeft, Physic.Move.State.MoveRight);

            void AddForceCOn()
            {
                ObjectState.State.HasAny(gameObject, Physic.Move.State.MoveLeft, Physic.Move.State.MoveRight);
                AddForce();
            }

            ObjectState.OnStateAdd.Add(gameObject, () =>
            {
                StopForce();
            }, Physic.Move.State.MoveLeft, Physic.Move.State.MoveRight);
            ObjectState.OnStateRemove.Add(gameObject, () =>
            {
                StopForce();
            }, GroundFinder.State.OnGround);

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
            ObjectState.State.Add(gameObject, State.GroundFriction);

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
            ObjectState.State.Remove(gameObject, State.GroundFriction);
        }
    }
}
