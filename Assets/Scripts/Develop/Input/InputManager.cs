using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static Global.Function;


public static class InputManager
{
    // * ---------------------------------- edit
    private static string inputActionAssetPath = "Profile/Input/InputActions";


    public enum InputName
    {
        Move,
        Jump,
        Attack,
        Shot,

    }
    // * ---------------------------------- 

    private static InputActionAsset inputActionAsset;

    public static InputActionAsset InputActionAsset
    {
        get
        {
            if (inputActionAsset == null)
            {
                inputActionAsset = (InputActionAsset)Resources.Load(inputActionAssetPath); ;
            }
            return inputActionAsset;
        }
    }

    private static bool enabled = false;

    public static void EnableAll()
    {
        if (enabled == true)
        {
            return;
        }

        foreach (string name in Enum.GetNames(typeof(InputName)))
        {
            InputActionAsset.FindAction(name).Enable();
        }

        enabled = true;
    }

    public static void DisableAll()
    {
        if (enabled == false)
        {
            return;
        }
        foreach (string name in Enum.GetNames(typeof(InputName)))
        {
            InputActionAsset.FindAction(name).Disable();
        }
        enabled = false;
    }


    // * ---------------------------------- 
    public static InputAction GetInputAction(InputName inputName)
    {
        if (enabled == false)
        {
            EnableAll();
        }
        return InputActionAsset.FindAction(inputName.ToString());
    }





}

