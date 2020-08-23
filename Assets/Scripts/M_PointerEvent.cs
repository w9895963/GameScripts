using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class M_PointerEvent : MonoBehaviour {

    public Vector2 lastPointerPosition;
    public bool lastPointerPositionSetup = false;
    public Vector2 pressBeginPosition;
    public bool lastPressDown = false;
    public bool onDrag = false;
    public Events events = new Events ();
    public PointerData data = new PointerData ();


    //*Main
    void Update () {
        Vector2 p = Pointer.current.position.ReadValue ();
        data.position = p;


        if (Pointer.current.press.isPressed) {
            if (lastPressDown == false) {
                lastPressDown = true;
                pressBeginPosition = p;
                data.pointerDownPosition = p;
                events.onPressDown.Invoke (data);
            } else {

                if (p != pressBeginPosition) {
                    if (onDrag == false) {
                        onDrag = true;
                        data.lastPosition = lastPointerPosition;
                        events.onDragBegin.Invoke (data);
                    }

                } else {
                    events.onHold.Invoke (data);
                }

                if (p != lastPointerPosition) {
                    data.lastPosition = lastPointerPosition;
                    events.onDrag.Invoke (data);
                }

            }
        } else {
            if (lastPressDown) {
                lastPressDown = false;
                events.onPressUp.Invoke (data);
                if (onDrag) {
                    onDrag = false;
                    data.lastPosition = lastPointerPosition;
                    events.onDragEnd.Invoke (data);


                } else if (pressBeginPosition == p) {
                    events.onClick.Invoke (data);
                }
            }
        }




        lastPointerPosition = p;
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
    }
    public class PointerData {
        public Vector2 pointerDownPosition;
        public Vector2 lastPosition;
        public Vector2 position;

    }
    public enum PointerEventType {
        onPressDown,
        onPressUp,
        onHold,
        onDragBegin,
        onDragEnd,
        onDrag,
        onClick
    }
}