using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FS_Grab : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Collider2D pointerTrigger;
    [SerializeField] private bool ignoreMass = true;
    [SerializeField] private bool pointForceMode = true;
    [SerializeField] private float force = 50;
    [SerializeField] private Vector2 curveMaxMin = new Vector2 (1, 0);
    [SerializeField] private AnimationCurve forceCurve = Global.Curve.ZeroOne;
    [SerializeField] private Events events = new Events ();
    [SerializeField] private EnableZone enableZone = new EnableZone ();
    [SerializeField, ReadOnly] private bool onDragging = false;
    [SerializeField, ReadOnly] private Vector2 target;
    private Vector2 beginPosition;




    private void OnValidate () {
        if (rigidbody == null) { rigidbody = GetComponent<Rigidbody2D> (); }
        if (pointerTrigger == null) { pointerTrigger = GetComponent<Collider2D> (); }
    }


    private void FixedUpdate () {
        bool inZoneTest = true;
        inZoneTest = enableZone.enable?enableZone.testResult : true;
        if (inZoneTest) {
            Main_Updata ();
        } else {
            onDragging = false;
        }


    }


    //*OnEvent
    private void Start () {
        Main_SetupPointerEvent ();

        if (enableZone.enable & enableZone.targetTriggerBox != null & enableZone.thisTriggerBox != null) {

            Collider2D targetBox = enableZone.targetTriggerBox;
            GameObject obj = targetBox.gameObject;

            obj.GetComponent<Collider2D> ().Ex_AddCollierEvent (
                enableZone.thisTriggerBox.gameObject.ToArray (),
                (d) => enableZone.testResult = true,
                (d) => enableZone.testResult = false
            );
        }

    }
    private void OnEnable () { }


    private void OnDisable () {
        onDragging = false;
    }

    //*Private Method
    private void Main_Updata () {
        if (onDragging) {
            Vector2 vector = target - rigidbody.GetRelativePoint (beginPosition);
            float index = (vector.magnitude - curveMaxMin[1]) / (curveMaxMin[0] - curveMaxMin[1]);


            Vector2 forceV = force * forceCurve.Evaluate (index) * vector.normalized;


            forceV *= (ignoreMass == true) ? rigidbody.mass : 1;

            if (pointForceMode) {
                rigidbody.AddForceAtPosition (forceV, rigidbody.GetRelativePoint (beginPosition));
            } else {
                rigidbody.AddForce (forceV);
            }
        }
    }
    private void Main_SetupPointerEvent () {
        var obj = pointerTrigger.gameObject;
        pointerTrigger.Ex_AddInputToTrigger (EventTriggerType.BeginDrag, (d) => {
            if (enabled) {
                PointerEventData data = d as PointerEventData;
                beginPosition = rigidbody.GetPoint (Camera.main.ScreenToWorldPoint (data.pressPosition));
                onDragging = true;
                events.dragBegin.Invoke ();
            }
        });
        pointerTrigger.Ex_AddInputToTrigger (EventTriggerType.Drag, (d) => {
            if (enabled) {
                PointerEventData data = d as PointerEventData;
                target = Camera.main.ScreenToWorldPoint (data.position);
            }

        });
        pointerTrigger.Ex_AddInputToTrigger (EventTriggerType.EndDrag, (d) => {
            if (enabled) {
                onDragging = false;
                events.dragEnd.Invoke ();
            }
        });
    }

    //*Property Class
    [System.Serializable]
    public class Events {
        public UnityEvent dragBegin = new UnityEvent ();
        public UnityEvent dragEnd = new UnityEvent ();
    }

    [System.Serializable]
    public class EnableZone {
        public bool enable = false;
        public Collider2D thisTriggerBox;
        public Collider2D targetTriggerBox;
        [ReadOnly]
        public bool testResult = false;
    }

}