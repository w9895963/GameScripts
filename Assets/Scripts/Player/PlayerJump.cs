using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Core core;

    private void Awake()
    {
        core = new Core(gameObject);
    }

    [System.Serializable]
    public class Core
    {
        public GameObject gameObject;
        public float jumpSpeed = 10;
        public float speedUpTime = 0.1f;
        public float jumpHeight = 4;
        public Vector2 jumpDirection = Vector2.up;
        public Action onJumpFinished;
        public Jump.Core core;

        public Core(GameObject gameObject)
        {
            this.gameObject = gameObject;
            core = new Jump.Core();
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


    }

}
