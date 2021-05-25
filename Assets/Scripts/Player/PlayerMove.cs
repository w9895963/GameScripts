using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 3f;
    public Main main = new Main();

    private void Awake()
    {
        main.gameObject = gameObject;
        main.Initial();

    }
    public class Main
    {
        public GameObject gameObject;
        public float maxSpeedOnGround = 10f;
        public float maxSpeedOnSky = 6f;
        public float maxForce = 100f;
        private Physic.Move.Core move;


        public void Initial()
        {
            move = new Physic.Move.Core(gameObject);
            InputManager.GetInputAction(InputManager.InputName.Move).performed += MoveInput;
            BasicEvent.OnDestroyEvent.Add(gameObject, () =>
            {
                InputManager.GetInputAction(InputManager.InputName.Move).performed -= MoveInput;
            });
        }

        private void MoveInput(UnityEngine.InputSystem.InputAction.CallbackContext d)
        {

            Vector2 dir = new Vector2(d.ReadValue<Vector2>().x, 0);
            if (dir.x != 0)
            {
                move.Move(dir);
            }
            else
            {
                move.Stop();
            }

        }
    }


}
