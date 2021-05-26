using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace CMDBundle
{
    namespace Component
    {
        public class EditorInitial : MonoBehaviour
        {
            public InputAction callSearchBar = new InputAction("callSearchBar", InputActionType.Button, "<Keyboard>/f6");

            void Start()
            {
                callSearchBar.performed += KeyAction;
                callSearchBar.Enable();
            }

            private void OnDestroy() {
                callSearchBar.performed -= KeyAction;
            }

            private void KeyAction(InputAction.CallbackContext d)
            {
                if (d.ReadValue<float>() > 0)
                {
                    Editor.CommandFileSearchBar.ToggleShowUp();
                }
            }
        }
    }
}
