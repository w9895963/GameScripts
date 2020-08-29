using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class C_PointerEvent : MonoBehaviour {

    public Vector2 lastPointerPosition;
    public bool lastPointerPositionSetup = false;
    public Vector2 pressBeginPosition;
    public bool lastPressDown = false;
    public bool onDrag = false;
    public Events events = new Events ();
    public PointerData data = new PointerData ();
    public float pointerDownTime = 0;
    public float clickAllowTime = 0.1f;
    public Component createBy = null;


    //*Main
    void Update () {
        Vector2 p = Pointer.current.position.ReadValue ();
        data.position_Screen = p;


        bool pressDown = Pointer.current.press.isPressed;
        if (pressDown) {
            if (lastPressDown == false) {
                lastPressDown = true;
                pressBeginPosition = p;
                data.pointerDownPosition = p;
                pointerDownTime = Time.unscaledTime;
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


                } else if (pressBeginPosition == p | Time.unscaledTime - pointerDownTime < clickAllowTime) {
                    events.onClick.Invoke (data);
                }
            }
        }

        if (p != lastPointerPosition & !pressDown & lastPointerPositionSetup) {
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
    public void AddEvent (PointerEventType type, UnityAction<PointerData> action) {

        switch (type) {
            case PointerEventType.onPressDown:
                events.onPressDown.AddListener (action);
                break;
            case PointerEventType.onPressUp:
                events.onPressUp.AddListener (action);
                break;
            case PointerEventType.onHold:
                events.onHold.AddListener (action);
                break;
            case PointerEventType.onDragBegin:
                events.onDragBegin.AddListener (action);
                break;
            case PointerEventType.onDragEnd:
                events.onDragEnd.AddListener (action);
                break;
            case PointerEventType.onClick:
                events.onClick.AddListener (action);
                break;
            case PointerEventType.onDrag:
                events.onDrag.AddListener (action);
                break;
            case PointerEventType.onMove:
                events.onMove.AddListener (action);
                break;
        }


    }
    //*Property
    [System.Serializable]
    public class Events {
        public UnityEvent<PointerData> onPressDown = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onPressUp = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onHold = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onDragBegin = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onDragEnd = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onDrag = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onClick = new UnityEvent<PointerData> ();
        public UnityEvent<PointerData> onMove = new UnityEvent<PointerData> ();
    }
}



public enum PointerEventType {
    onPressDown,
    onPressUp,
    onHold,
    onDragBegin,
    onDragEnd,
    onDrag,
    onClick,
    onMove
}



public class PointerData {
    public Vector2 pointerDownPosition;
    public Vector2 lastPosition_Screen;
    public Vector2 position_Screen;

}


public static class _Extention_M_PointerEvent {
    public static GameObject Ex_AddPointerEvent (this Component component,
        PointerEventType type,
        UnityAction<PointerData> action) {


        GameObject obj = new GameObject ("Pointer Event");
        C_PointerEvent comp = obj.AddComponent<C_PointerEvent> ();
        comp.createBy = component;
        comp.AddEvent (type, action);
        return obj;
    }
    public static GameObject AddPointerEvent (this Fn fn,
        PointerEventType type,
        UnityAction<PointerData> action) {


        GameObject obj = new GameObject ("Pointer Event");
        C_PointerEvent comp = obj.AddComponent<C_PointerEvent> ();
        comp.AddEvent (type, action);
        return obj;
    }
    public static GameObject Ex_AddPointerEventOnece (this Component component,
        PointerEventType type,
        UnityAction<PointerData> action) {


        GameObject obj = new GameObject ("Pointer Event");
        C_PointerEvent comp = obj.AddComponent<C_PointerEvent> ();
        comp.createBy = component;
        UnityAction<PointerData> ac = (d) => {
            action (d);
            GameObject.Destroy (obj);
        };
        comp.AddEvent (type, ac);
        return obj;
    }
}