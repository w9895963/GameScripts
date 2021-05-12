using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 3f;

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


}
