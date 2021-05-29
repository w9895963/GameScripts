using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EditorMode : MonoBehaviour
{
    public InputAction reloadScene = new InputAction("key", InputActionType.Button, "<Keyboard>/f5");
    public bool reload = false;
    public bool controlCamera = false;
    private void Awake()
    {
        if (reload)
        {
            reloadScene.performed += Reload;
            reloadScene.Enable();
            BasicEvent.OnDestroyEvent.Add(gameObject, () =>
            {
                reloadScene.performed -= Reload;
            });
        }

        if (controlCamera)
        {
            Camera.main.gameObject.AddComponent<CameraMouseDrag>();
            Camera.main.gameObject.AddComponent<CameraMouseZoom>();
            Camera.main.gameObject.GetComponent<CameraDefaultBehaviour>().Destroy();
            Camera.main.gameObject.GetComponent<BasicEvent.Component.OnFixedUpdateComponent>().Destroy();
        }
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
