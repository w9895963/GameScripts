using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDataMapper<T> where T : struct
{

    public Action onInputAction;
    public T inputData;
    private InputAction inputAction;
    private bool enabled = false;
    private GameObject gameObject;
    private ObjectDataName objectDataName;
    private T defaultData;

    public PlayerInputDataMapper(GameObject gameObject, InputManager.InputName inputname, ObjectDataName dataname, T defaultData = default)
    {
        inputAction = InputManager.GetInputAction(inputname);
        this.gameObject = gameObject;
        this.objectDataName = dataname;
        this.defaultData = defaultData;
        Enabled = true;
    }




    public void InputActionMove(InputAction.CallbackContext d)
    {
        inputData = d.ReadValue<T>();
        onInputAction?.Invoke();
        ObjectDate.UpdateDate(gameObject, objectDataName, inputData);
    }

    public bool Enabled
    {
        get => enabled;
        set
        {
            if (value == enabled)
            {
                return;
            }

            enabled = value;



            if (enabled == true)
            {
                inputAction.performed += InputActionMove;
            }
            else
            {
                inputData = defaultData;
                inputAction.performed -= InputActionMove;
            }
        }
    }
}
