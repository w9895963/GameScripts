using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Physic
{
    public class Move : MonoBehaviour
    {
        public Vector2 moveDirection = Vector2.zero;

        public float moveSpeed = 10f;
        public float maxForce = 100f;
        public Vector2 groundNormal = Vector2.up;

        public Vector2 realWalkDirection;
        public Vector2 force;


        private Core core;
        private Rigidbody2D rigidBody => gameObject.GetComponent<Rigidbody2D>();


        private void Awake()
        {
            core = new Core(gameObject);

            var c = core;

            c.onVariablesChanged += () =>
            {
                moveDirection = c.moveDirection;
            };
            c.onUpdateBefore += () =>
            {
                c.moveDirection = moveDirection;
                c.maxSpeed = moveSpeed;
                c.groundNormal = groundNormal;
            };
            c.onUpdateAfter += () =>
            {
                realWalkDirection = c.realWalkDirection;
                force = c.force;
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
            public Vector2 moveDirection = Vector2.zero;

            public float maxSpeed = 10f;
            public float maxForce = 100f;

            public Vector2 groundNormal = Vector2.up;

            public Action onVariablesChanged;
            public Action onUpdateBefore;
            public Action onUpdateAfter;

            public Vector2 realWalkDirection;
            public Vector2 force;




            private Physic.VelocityChanger.Core velocityChanger;
            private GameObject gameObject;
            private Rigidbody2D rigidBody => gameObject.GetComponent<Rigidbody2D>();
            private bool enabled = false;
            public Core(GameObject gameObject)
            {
                this.gameObject = gameObject;
                velocityChanger = new Physic.VelocityChanger.Core(rigidBody);
                ObjectDate.AddListener(gameObject, ObjectDataName.MoveDirection, (d) =>
                {
                    moveDirection = (Vector2)d;
                    onVariablesChanged?.Invoke();
                });

            }


            public void FixedUpdateAction()
            {
                onUpdateBefore?.Invoke();
                realWalkDirection = moveDirection.ProjectOnPlane(groundNormal).normalized;
                Vector2 currV = rigidBody.velocity.ProjectOnPlane(groundNormal);
                Vector2 tarV = realWalkDirection * maxSpeed;
                var vc = velocityChanger;

                vc.singleDirection = groundNormal.Rotate(90);
                vc.maxForce = maxForce;
                vc.targetVelocity = tarV;

                vc.FixedUpdateAction();

                force = vc.forceAdd;
                onUpdateAfter?.Invoke();


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


