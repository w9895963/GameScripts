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
        // * ---------------------------------- edit
        private static string inputActionAssetPath = "Profile/Input/InputActions";
        [System.Serializable]
        public class InputActionState
        {
            public Vector2 move;
            public float down;
            public float jump;
            public float attack;
            public float shot;
        }
        public enum InputName
        {
            Move,
            Jump,
            Attack,
            Shot
        }
        // * ---------------------------------- 
        private static InputActionAsset inputActionAsset;
        public static InputActionState keyState;



        // * ---------------------------------- 




        public static void Initial(InputActionState keyState)
        {
            InputManager.keyState = keyState;
            inputActionAsset = (InputActionAsset)Resources.Load(inputActionAssetPath);

            GetInputAction(InputName.Move).performed += (d) =>
            {
                keyState.move = d.ReadValue<Vector2>();

            };
            GetInputAction(InputName.Jump).performed += (d) =>
            {
                keyState.jump = d.ReadValue<float>();
            };
            GetInputAction(InputName.Attack).performed += (d) =>
            {
                keyState.attack = d.ReadValue<float>();
            };

        }



        // * ---------------------------------- 


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
