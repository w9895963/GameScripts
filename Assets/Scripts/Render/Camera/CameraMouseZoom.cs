using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseZoom : MonoBehaviour
{
    public float scrollFactor = 0.0004f;

    void Update()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            Camera cam = GetComponent<Camera>();
            cam.orthographicSize -= scroll * scrollFactor * cam.orthographicSize;
            DateF.AddDate<Date.Camera.Size, float>(Camera.main, Camera.main.orthographicSize);
        }
    }
}
