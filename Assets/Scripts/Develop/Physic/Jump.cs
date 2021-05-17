using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Core core = new Core();

    private void Awake()
    {
        core.gameObject = gameObject;
        InputManager.GetInputAction(InputManager.InputName.Jump).performed += (d) =>
        {
            float v = d.ReadValue<float>();
            if (v == 1)
            {
                core.StartJump();
            }
            else
            {
                core.StopJump();
            }

        };
    }




    [System.Serializable]
    public class Core
    {
        public GameObject gameObject;
        public float jumpSpeed = 20;
        public float jumpHeight = 4f;
        public float slowDownHeight = 1;
        public float maxJumpForce = 600;
        public float maxSlowDownForce = 200;
        public float slowDownRate = 0.5f;
        public Vector2 jumpDirection = Vector2.up;
        public float stopSpeedCondition = 0.1f;
        public Action onJumpFinished;

        public int step = 0;
        public Vector2 startPosition;
        public float CurrHeight => (body.position - startPosition).ProjectToFloat(jumpDirection);
        public float RemainHeight => jumpHeight - CurrHeight;

        public Rigidbody2D body => gameObject.GetRigidbody2D();
        public float currSpeed => body.velocity.ProjectToFloat(jumpDirection);
        public float mass => body.mass;




        private bool jumpEnabled = false;


        public void JumpInitial()
        {
            step = 0;
            startPosition = body.position;
        }

        public void FixedUpdateAction()
        {
            float currForce = 0;

            if (CurrHeight < slowDownHeight)
            {
                float forceNeed = MathPh.SpeedChangeForce(currSpeed, jumpSpeed, mass);
                currForce = forceNeed.Clamp(0, maxJumpForce);
            }
            else
            {
                float targetSpeed = (RemainHeight.ClampMin(0) / (jumpHeight - slowDownHeight)).PowSafe(slowDownRate) * jumpSpeed;
                float forceNeed = MathPh.SpeedChangeForce(currSpeed, targetSpeed, mass);
                currForce = forceNeed.Clamp(-maxSlowDownForce, maxSlowDownForce);
                if ((body.velocity).magnitude < stopSpeedCondition)
                {
                    StopJump();
                }
            }




            body.AddForce(currForce * jumpDirection.normalized);
            step++;
        }


        public void StartJump()
        {
            if (jumpEnabled == false)
            {
                jumpEnabled = true;
                JumpInitial();
                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
            }
        }

        public void StopJump()
        {
            if (jumpEnabled == true)
            {
                jumpEnabled = false;
                BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
                onJumpFinished?.Invoke();
            }
        }


    }
}
