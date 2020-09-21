using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class C_MouseEvent : MonoBehaviour {

    public Vector2 lastPointerPosition;
    public bool lastPointerPositionSetup = false;
    public Vector2 pressBeginPosition;
    public bool lastPressDown = false;
    public bool onDrag = false;
    public Events events = new Events ();
    public MouseData data = new MouseData ();
    public float mouseDownTime = 0;
    public float clickAllowTime = 0.1f;
    public Component createBy = null;


    //*Main
    void Update () {
        Vector2 p = Mouse.current.position.ReadValue ();
        data.position_Screen = p;
        data.delta = Mouse.current.delta.ReadValue ();



        bool pressDown = Mouse.current.press.isPressed;
        if (pressDown) {
            if (lastPressDown == false) {
                lastPressDown = true;
                pressBeginPosition = p;
                data.pressDownPosition = p;
                mouseDownTime = Time.unscaledTime;
                events.onPressDown.Invoke (data);
            } else {

                if (p != pressBeginPosition) {
                    if (onDrag == false) {
                        onDrag = true;
                        events.onDragBegin.Invoke (data);
                    }

                } else {
                    events.onHold.Invoke (data);
                }

                if (onDrag) {
                    events.onDrag.Invoke (data);
                }

            }
        } else {
            if (lastPressDown) {
                lastPressDown = false;
                events.onPressUp.Invoke (data);
                if (onDrag) {
                    onDrag = false;
                    events.onDragEnd.Invoke (data);


                } else if (pressBeginPosition == p | Time.unscaledTime - mouseDownTime < clickAllowTime) {
                    events.onClick.Invoke (data);
                }
            }
        }

        if (Mouse.current.delta.ReadValue ().magnitude > 0) {
            events.onMove.Invoke (data);
        }




        lastPointerPosition = p;
        data.lastPosition_Screen = p;
        lastPointerPositionSetup = true;
    }
    private void OnDisable () {
        lastPointerPositionSetup = false;
    }
    //*Public Method
    public void AddEvent (MouseEventType type, UnityAction<MouseData> action) {

        switch (type) {
            case MouseEventType.onPressDown:
                events.onPressDown.AddListener (action);
                break;
            case MouseEventType.onPressUp:
                events.onPressUp.AddListener (action);
                break;
            case MouseEventType.onHold:
                events.onHold.AddListener (action);
                break;
            case MouseEventType.onDragBegin:
                events.onDragBegin.AddListener (action);
                break;
            case MouseEventType.onDragEnd:
                events.onDragEnd.AddListener (action);
                break;
            case MouseEventType.onClick:
                events.onClick.AddListener (action);
                break;
            case MouseEventType.onDrag:
                events.onDrag.AddListener (action);
                break;
            case MouseEventType.onMove:
                events.onMove.AddListener (action);
                break;
        }


    }
    //*Property
    [System.Serializable]
    public class Events {
        public UnityEvent<MouseData> onPressDown = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onPressUp = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onHold = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onDragBegin = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onDragEnd = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onDrag = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onClick = new UnityEvent<MouseData> ();
        public UnityEvent<MouseData> onMove = new UnityEvent<MouseData> ();
    }
}

public enum MouseEventType {
    onPressDown,
    onPressUp,
    onHold,
    onDragBegin,
    onDragEnd,
    onDrag,
    onClick,
    onMove
}


public class MouseData {
    public Vector2 pressDownPosition;
    public Vector2 lastPosition_Screen;
    public Vector2 position_Screen;
    public Vector2 delta;

}


public static class Extension_M_MouseEvent {
    public static GameObject Ex_AddMouseEvent (this Component component,
        MouseEventType type,
        UnityAction<MouseData> action) {


        GameObject obj = new GameObject ("Mouse Event");
        C_MouseEvent comp = obj.AddComponent<C_MouseEvent> ();
        comp.createBy = component;
        comp.AddEvent (type, action);
        return obj;
    }
    public static GameObject Ex_AddMouseEventOnece (this Component component,
        MouseEventType type,
        UnityAction<MouseData> action) {


        GameObject obj = new GameObject ("Pointer Event");
        C_MouseEvent comp = obj.AddComponent<C_MouseEvent> ();
        comp.createBy = component;
        UnityAction<MouseData> ac = (d) => {
            action (d);
            GameObject.Destroy (obj);
        };
        comp.AddEvent (type, ac);
        return obj;
    }
}