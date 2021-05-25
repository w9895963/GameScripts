using System.Collections;
using System.Collections.Generic;
using BasicEvent;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseDrag : MonoBehaviour
{
    public float moveFacter = 0.004f;

 /*    private void FixedUpdate()
    {
        var leftButton = Mouse.current.leftButton.ReadValue();
        if (leftButton > 0)
        {
            if (InputBundle.SelectedObject.current != null) return;
            Vector2 del = Mouse.current.delta.ReadValue();
            Vector2 curr = gameObject.GetPosition2d();
            gameObject.SetPosition(curr + del * -moveFacter * Camera.main.orthographicSize);
        }
    } */

    private void Start()
    {
        BasicEvent.OnPointerDrag.EmptyDrag(onDrag, onEnd, onStart);
    }

    private void onStart(OnPointerDrag.DragDate d)
    {
    }

    private void onEnd(OnPointerDrag.DragDate d)
    {

    }

    private void onDrag(OnPointerDrag.DragDate d)
    {
        Vector2 del = d.screenDelta;
        Vector2 curr = gameObject.GetPosition2d();
        gameObject.SetPosition(curr + del * -moveFacter * Camera.main.orthographicSize);
    }
}
