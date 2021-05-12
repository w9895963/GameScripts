using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseDrag : MonoBehaviour
{
    public float moveFacter = 0.02f;

    private void FixedUpdate()
    {
        var leftButton = Mouse.current.leftButton.ReadValue();
        if (leftButton > 0)
        {
            Vector2 del = Mouse.current.delta.ReadValue();
            Vector2 curr = gameObject.GetPosition2d();
            gameObject.SetPosition(curr + del * moveFacter);
        }
    }



}
