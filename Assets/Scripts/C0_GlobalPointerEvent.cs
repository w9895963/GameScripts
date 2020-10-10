using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class C0_GlobalPointerEvent : MonoBehaviour {

    public Vector2 lastPointerPosition;
    public bool lastPointerPositionSetup = false;
    public Vector2 pressBeginPosition;
    public bool lastPressDown = false;
    public bool onDrag = false;
    public Events events = new Events ();
    public PointerData data = new PointerData ();
    public float pointerDownTime = 0;
    public float clickAllowTime = 0.3f;
    public Object createBy = null;


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

                if (p != pressBeginPosition & Time.unscaledTime - pointerDownTime >= clickAllowTime) {
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
            events.onMoveNotDrag.Invoke (data);
        }


        if (p != lastPointerPosition & lastPointerPositionSetup) {
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
            case PointerEventType.onMoveNotDrag:
                events.onMoveNotDrag.AddListener (action);
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
        public UnityEvent<PointerData> onMoveNotDrag = new UnityEvent<PointerData> ();
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
    onMoveNotDrag,
    onMove
}


public class PointerData {
    public Vector2 pointerDownPosition;
    public Vector2 lastPosition_Screen;
    public Vector2 position_Screen;

}


public static class Extension_GlobalPointerEvent {
    public static GameObject AddGlobalPointerEvent (this Global.Funtion fn,
        PointerEventType type,
        UnityAction<PointerData> action) {

        const string Name = "Pointer Event";
        GameObject obj = GlobalObject.TempObject.FindChild (Name);
        if (obj == null) {
            obj = new GameObject (Name);
            obj.SetParent (GlobalObject.TempObject);
        }
        C0_GlobalPointerEvent comp = obj.AddComponent<C0_GlobalPointerEvent> ();
        comp.createBy = fn.callBy;
        comp.AddEvent (type, action);
        return obj;
    }
}