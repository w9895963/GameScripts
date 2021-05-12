using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Physic
{
    public class Gravity : MonoBehaviour
    {
        public Vector2 gravityForce = new Vector2(0, -80);


        private void Awake()
        {
            ObjectState.OnStateAdd.Add(gameObject, ObjectStateName.Jump, () =>
            {
                enabled = false;
            });
            ObjectState.OnStateRemove.Add(gameObject, ObjectStateName.Jump, () =>
            {
                enabled = true;
            });
        }

        private void OnEnable()
        {
            BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
            ObjectState.State.Add(gameObject, ObjectStateName.Gravity);
        }

        private void OnDisable()
        {
            BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
            ObjectState.State.Remove(gameObject, ObjectStateName.Gravity);
        }

        private void FixedUpdateAction()
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(gravityForce);
        }
    }
}
