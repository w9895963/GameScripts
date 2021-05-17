using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 3f;
    public Main main = new Main();

    private void Awake()
    {
        Physic.Move.Core core = new Physic.Move.Core(gameObject);
        core.Enabled = true;
        core.maxSpeed = maxSpeed;
        InputManager.GetInputAction(InputManager.InputName.Move).performed += (d) =>
        {
            core.moveDirection = d.ReadValue<Vector2>();
        };

    }
    public class Main
    {
        public GameObject gameObject;
        public float maxSpeedOnGround = 10f;
        public float maxSpeedOnSky = 6f;
        public float maxForce = 100f;
        private Physic.Move.Core move;


        private bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value)
                {
                    return;
                }
                enabled = value;

                if (enabled == true)
                {
                    move.Enabled = true;
                }
                else
                {
                    move.Enabled = false;
                }
            }
        }

        public void Initial()
        {
            move = new Physic.Move.Core(gameObject);
        }

    }


}
