using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physic
{
    public class VelocityChanger : MonoBehaviour
    {

        public Vector2 targetVelocity;

        public float maxForce = 100f;
        public Vector2 singleDirection = Vector2.zero;

        public Action onBeforeUpdate;
        public Action onAfterUpdate;


        public Vector2 forceAdd;
        private Core core;
        private Rigidbody2D rigidBody => gameObject.GetComponent<Rigidbody2D>();
        private void Awake()
        {
            core = new Core(rigidBody);
            var c = core;
            core.onBeforeUpdate += () =>
              {
                  onBeforeUpdate?.Invoke();
                  c.targetVelocity = targetVelocity;
                  c.maxForce = maxForce;
                  c.singleDirection = singleDirection;
              };
            core.onAfterUpdate += () =>
            {
                forceAdd = c.forceAdd;
                onAfterUpdate?.Invoke();
            };
        }
        private void OnEnable()
        {
            core.Enabled = true;
        }
        private void OnDisable()
        {
            core.Enabled = false;
        }




        public class Core
        {

            public Rigidbody2D rigidbody;
            public GameObject gameObject => rigidbody.gameObject;

            public Vector2 targetVelocity;

            public float maxForce = 100f;
            public Vector2 singleDirection = Vector2.zero;

            public Action onBeforeUpdate;
            public Action onAfterUpdate;


            public Vector2 forceAdd;

            private bool enabled = false;

            public Core(Rigidbody2D rigidbody)
            {
                this.rigidbody = rigidbody;
            }

            public void FixedUpdateAction()
            {
                onBeforeUpdate?.Invoke();
                Vector2 currentVelocity = rigidbody.velocity;
                Vector2 delV = targetVelocity - currentVelocity;
                delV = singleDirection.IsNotZero() ? delV.Project(singleDirection) : delV;
                Vector2 topForce = delV / Time.fixedDeltaTime * rigidbody.mass;
                forceAdd = topForce.ClampDistanceMax(maxForce);
                rigidbody.AddForce(forceAdd);
                onAfterUpdate?.Invoke();
            }


            public bool Enabled
            {
                get
                {
                    return enabled;
                }
                set
                {
                    if (value != enabled)
                    {
                        if (value == true)
                        {
                            BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
                        }
                        else
                        {
                            BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
                        }
                    }

                }
            }





        }


    }
}
