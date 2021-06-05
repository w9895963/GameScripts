using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EditorMode : MonoBehaviour
{
    public InputAction reloadScene = new InputAction("key", InputActionType.Button, "<Keyboard>/f5");
    [SerializeField]
    private bool controlCamera = true;


    public bool ControlCamera
    {
        get => controlCamera;
        set
        {
            if (controlCamera == value) return;
            controlCamera = value;
            if (controlCamera == true)
            {
                CamareF.TakeControl();
            }
            else
            {
                CamareF.FollowPlayer();
            }


        }
    }

    private static void Control()
    {
        Camera.main.gameObject.AddComponent<CameraMouseDrag>();
        Camera.main.gameObject.AddComponent<CameraMouseZoom>();
        Camera.main.gameObject.GetComponent<CameraDefaultBehaviour>().Destroy();
        Camera.main.gameObject.GetComponent<BasicEvent.Component.OnFixedUpdateComponent>().Destroy();
    }

    private void Awake()
    {
        reloadScene.performed += Reload;
        reloadScene.Enable();
        BasicEvent.OnDestroyEvent.Add(gameObject, () =>
        {
            reloadScene.Disable();
            reloadScene.performed -= Reload;
        });




    }

    private void Start()
    {
        controlCamera = StateF.GetState<CameraState.CameraControl>(Camera.main.gameObject);
    }

    private void Reload(InputAction.CallbackContext d)
    {
        float v = d.ReadValue<float>();
        if (v > 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
