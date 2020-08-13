using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FS_Grab : MonoBehaviour {
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private Collider2D pointerTrigger;
    [SerializeField]
    private bool ignoreMass = true;
    [SerializeField]
    private bool pointForceMode = true;
    [SerializeField]
    private float force = 50;
    [SerializeField]
    private Vector2 curveMaxMin = new Vector2 (0, 1);
    [SerializeField]
    private AnimationCurve forceCurve = default;
    [SerializeField]
    private Events events = new Events ();
    [SerializeField, ReadOnly]
    private bool onDragging = false;
    [SerializeField, ReadOnly]
    private Vector2 target;
    private Vector2 beginPosition;




    private void OnValidate () {
        if (rigidbody == null) { rigidbody = GetComponent<Rigidbody2D> (); }
        if (pointerTrigger == null) { pointerTrigger = GetComponent<Collider2D> (); }
    }


    private void FixedUpdate () {
        if (onDragging) {
            Fn.DrawCross (rigidbody.GetRelativePoint (beginPosition));
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

    private void OnEnable () {
        var obj = pointerTrigger.gameObject;
        Fn.AddEventToTrigger (obj, EventTriggerType.BeginDrag, (d) => {
            if (enabled) {
                PointerEventData data = d as PointerEventData;
                beginPosition = rigidbody.GetPoint (Camera.main.ScreenToWorldPoint (data.pressPosition));
                onDragging = true;
                events.dragBegin.Invoke ();
            }
        });
        Fn.AddEventToTrigger (obj, EventTriggerType.Drag, (d) => {
            if (enabled) {
                PointerEventData data = d as PointerEventData;
                target = Camera.main.ScreenToWorldPoint (data.position);
            }

        });
        Fn.AddEventToTrigger (obj, EventTriggerType.EndDrag, (d) => {
            if (enabled) {
                onDragging = false;
                events.dragEnd.Invoke ();
            }
        });
    }


    private void OnDisable () {
        onDragging = false;
    }

    [System.Serializable]
    public class Events {
        public UnityEvent dragBegin = new UnityEvent ();
        public UnityEvent dragEnd = new UnityEvent ();
    }

}