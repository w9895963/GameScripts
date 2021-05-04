using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputDataMapper<Vector2> move;

        private void Awake()
        {
            move = new PlayerInputDataMapper<Vector2>(gameObject, InputManager.InputName.Move, ObjectDataName.MoveDirection, Vector2.zero);

        }
        private void OnEnable()
        {
            move.Enabled = true;
        }
        private void OnDisable()
        {
            move.Enabled = false;
        }

    }
}

