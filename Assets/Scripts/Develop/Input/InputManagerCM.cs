using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class InputManagerCM : MonoBehaviour
{
    public InputManager.InputActionState keyState = new InputManager.InputActionState();

    private void Awake()
    {
        InputManager.Initial(keyState);
    }
    private void OnEnable()
    {
        InputManager.EnableAll();
    }
    private void OnDisable()
    {
        InputManager.DisableAll();
    }
}