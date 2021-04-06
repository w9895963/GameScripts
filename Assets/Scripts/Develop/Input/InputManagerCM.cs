using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class InputManagerCM : MonoBehaviour
{

    private void Awake()
    {
        InputManager.Initial();
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