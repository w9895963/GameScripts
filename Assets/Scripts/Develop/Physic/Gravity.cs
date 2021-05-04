using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Physic
{
    public class Gravity : MonoBehaviour
    {
        public Vector2 gravityForce = new Vector2(0, -80);

        private void OnEnable()
        {
            BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
        }

        private void OnDisable()
        {
            BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
        }

        private void FixedUpdateAction()
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(gravityForce);
        }
    }
}
