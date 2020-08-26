using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class M_Grab : MonoBehaviour {
    [Header ("Dependent Component")]
    public Rigidbody2D rigidBody = null;
    public Collider2D clickBox = null;
    [Header ("Setting")]
    public float force = 80f;
    public AnimationCurve forceDistanceCurve = Fn.Curve.OneOneCurve;
    public float curveMaxDistance = 1f;
    public Vector2 pointForcePosition = default;
    public float walkSpeed = 2f;
    public float maxGrabDistance = 4f;
    public TriggerZone triggerZone = new TriggerZone ();
    public LinkToObject linkToObject = new LinkToObject ();
    [SerializeField, ReadOnly] private C1_TargetForce targetForceComp = null;
    [SerializeField, ReadOnly] private bool onGrab = false;
    [SerializeField, ReadOnly] private Vector2 targetPoint = Vector2.zero;
    private EventTrigger clickTrigger = null;
    private C_PointerEvent ev;
    private C_PointerEvent ev1;
    private C_PointerEvent ev2;
    private UnityEngine.Object[] cellection = new UnityEngine.Object[0];

    void Awake () {

        if (triggerZone.enable) {
            triggerZone.trigerZone.Ex_AddTriggerEvent (gameObject,
                (d) => {
                    Setup ();
                }, (d) => {
                    StopGrab ();
                });
        } else {
            Setup ();
        }

    }

    private void Break () {
        StopGrab ();
    }

    private void OnDisable () {
        StopGrab ();
    }

    private void FixedUpdate () {

        if (onGrab) {

            targetForceComp.SetTarget (targetPoint);
            Fn._.DrawPoint (targetPoint);

        }


    }




    //* Private Method
    private void StartGrab () {
        onGrab = true;
        targetPoint = rigidBody.position;
        Cursor.visible = false;

        if (!targetForceComp) {
            targetForceComp = this.Ex_AddTargetForce (targetPoint,
                force,
                pointForcePosition,
                forceCurve : forceDistanceCurve,
                curveMaxDistance : curveMaxDistance);
        }
        if (linkToObject.enable) {
            DistanceJoint2D ds = gameObject.AddComponent<DistanceJoint2D> ();
            ds.connectedBody = linkToObject.gameObject.GetComponent<Rigidbody2D> ();
            ds.enableCollision = true;
            ds.autoConfigureDistance = false;
            ds.distance = linkToObject.distance;
            ds.maxDistanceOnly = true;

            linkToObject.jointComp = ds;

        }


    }
    private void StopGrab () {
        onGrab = false;
        Cursor.visible = true;
        if (targetForceComp) {
            Destroy (targetForceComp);
        }
        if (ev)
            Destroy (ev.gameObject);
        if (ev1)
            Destroy (ev1.gameObject);
        if (clickTrigger)
            Destroy (clickTrigger);
        if (ev2)
            Destroy (ev2.gameObject);

        if (linkToObject.jointComp) {
            Destroy (linkToObject.jointComp);
        }
    }

    private void Setup () {
        clickTrigger = clickTrigger? clickTrigger : this.Ex_AddInputEventToTriggerOnece (clickBox, EventTriggerType.PointerClick, (d) => {
            StartGrab ();



            ev = ev?ev : this.AddPointerEvent (PointerEventType.onDrag, (d2) => {
                Vector2 vector = d2.position.ScreenToWold () - d2.lastPosition.ScreenToWold ();
                targetPoint += vector;
            });
            ev1 = ev1?ev1 : this.AddPointerEvent (PointerEventType.onMove, (d2) => {
                Vector2 vector = d2.position.ScreenToWold () - d2.lastPosition.ScreenToWold ();
                targetPoint += vector;
            });

            ev2 = ev2?ev2 : this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {

                StopGrab ();
                Setup ();
            });

        });



    }



    //*Property
    [System.Serializable]
    public class TriggerZone {
        public bool enable = false;
        public Collider2D trigerZone = null;
        [ReadOnly] public bool onTriggerZone = false;


    }

    [System.Serializable]
    public class LinkToObject {
        public bool enable = false;
        public GameObject gameObject;
        public float distance = 1f;
        [ReadOnly] public DistanceJoint2D jointComp;

    }

}