using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Physic
{
    public class Move : MonoBehaviour
    {
        public enum State { MoveLeft, MoveRight }
        public Vector2 moveDirection = Vector2.zero;

        public float moveSpeed = 10f;
        public float maxForce = 100f;
        public Vector2 groundNormal = Vector2.up;

        public Vector2 realWalkDirection;
        public Vector2 force;


        private Core core;


        private void Awake()
        {
            core = new Core(gameObject);

            var c = core;

            c.onUpdateBefore += () =>
            {
                c.moveDirection = moveDirection;
                c.maxSpeed = moveSpeed;
                c.maxForce = maxForce;
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
        }
        private void OnDisable()
        {

        }


        public class Core
        {
            public Vector2 moveDirection = Vector2.zero;

            public float maxSpeed = 10f;
            public float maxForce = 100f;

            public Vector2 groundNormal = Vector2.up;

            public Action onUpdateBefore;
            public Action onUpdateAfter;

            public Vector2 realWalkDirection;
            public Vector2 force;
            public bool enabled = false;




            private Physic.VelocityChanger.Core velocityChanger;
            private GameObject gameObject;
            private Rigidbody2D rigidBody => gameObject.GetComponent<Rigidbody2D>();
            public Core(GameObject gameObject)
            {
                this.gameObject = gameObject;
                velocityChanger = new Physic.VelocityChanger.Core(rigidBody);
                InitialState();
                InitialDateShare();
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
            private void InitialDateShare()
            {
                ObjectDate.OnDateUpdate(gameObject, GroundFinder.Date.GroundNormal, (d) =>
                {
                    Vector2 normal = (Vector2)d;
                });
            }
            private void InitialState()
            {

            }

            private void UpdateState()
            {
                if (enabled)
                {
                    if (moveDirection.x > 0)
                    {
                        ObjectState.State.Remove(gameObject, State.MoveLeft);
                        ObjectState.State.Add(gameObject, State.MoveRight);
                    }
                    else if (moveDirection.x < 0)
                    {
                        ObjectState.State.Remove(gameObject, State.MoveRight);
                        ObjectState.State.Add(gameObject, State.MoveLeft);
                    }
                }
                else
                {
                    ObjectState.State.Remove(gameObject, State.MoveLeft, State.MoveRight);
                }
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

            public void Move(Vector2 direction)
            {
                if (enabled) { return; }
                if (direction == Vector2.zero) { return; }
                enabled = true;
                moveDirection = direction;
                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
                UpdateState();
            }

            public void Stop()
            {
                if (!enabled) { return; }
                enabled = false;
                BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
                UpdateState();
            }



        }



    }
}


