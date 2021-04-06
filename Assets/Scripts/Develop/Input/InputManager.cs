using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static Global.Function;

namespace Global
{
    public static class InputManager
    {
        private static string inputActionAssetPath = "Profile/Input/InputActions";
        private static InputActionAsset inputActionAsset;



        public enum InputName
        {
            Move,
            Jump,
            Attack
        }




        public static void Initial()
        {
            inputActionAsset = (InputActionAsset)Resources.Load(inputActionAssetPath);

        }
        public static void EnableAll()
        {
            foreach (string name in Enum.GetNames(typeof(InputName)))
            {
                inputActionAsset.FindAction(name).Enable();
            }



        }

        public static void DisableAll()
        {
            foreach (string name in Enum.GetNames(typeof(InputName)))
            {
                inputActionAsset.FindAction(name).Disable();
            }
        }

        public static InputAction GetInputAction(InputName inputName)
        {
            return inputActionAsset.FindAction(inputName.ToString());
        }
    }
}
