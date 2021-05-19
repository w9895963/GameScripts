using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EditorMode : MonoBehaviour
{
    public InputAction reloadScene = new InputAction("key", InputActionType.Button, "<Keyboard>/f5");
    private void Awake()
    {
        reloadScene.performed += (d) =>
        {
            float v = d.ReadValue<float>();
            if (v > 0)
            {
                SceneManager.LoadScene(0);
            }
        };
        reloadScene.Enable();
        Camera.main.gameObject.AddComponent<CameraMouseDrag>();
        Camera.main.gameObject.AddComponent<CameraMouseZoom>();
        Camera.main.gameObject.GetComponent<CameraDefaultBehaviour>().Destroy();
        Camera.main.gameObject.GetComponent<BasicEvent.Component.OnFixedUpdateComponent>().Destroy();
    }
}
