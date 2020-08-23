using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class M_Grab : MonoBehaviour {
    [Header ("Dependent Component")]
    public Rigidbody2D rigidBody = null;
    public Collider2D triggerZone = null;
    public Collider2D clickBox = null;
    [Header ("Setting")]
    public float force = 80f;
    public Vector2 pointForcePosition = default;
    public float maxSpeed = 2f;
    public float maxGrabDistance = 4f;
    [SerializeField, ReadOnly] private FS_PointForce pointForce = null;
    [SerializeField, ReadOnly] private bool onGrab = false;
    [SerializeField, ReadOnly] private bool onTriggerZone = false;

    void Awake () {
        if (pointForce == null) {
            pointForce = gameObject.AddComponent<FS_PointForce> ();
            FS_PointForce.Property pr = new FS_PointForce.Property ();
            pr.force = force;
            pr.pointForceMode = true;
            pr.pointForcePosition = pointForcePosition;
            pointForce.SetUp (pr);
            pointForce.enabled = false;
        }


        Fn.AddEventToTrigger (clickBox.gameObject, EventTriggerType.PointerClick, (d) => {
            if (enabled & onTriggerZone) {
                onGrab = true;
                pointForce.enabled = true;



                M_PointerEvent click = null;
                click = Fn.AddPointerEvent (M_PointerEvent.PointerEventType.onClick, (d2) => {
                    StopGrab ();
                    Destroy (click?.gameObject);
                });
            }
        });


        M_TriggerEvent ev = clickBox.gameObject.AddComponent<M_TriggerEvent> ();
        ev.SetObject (triggerZone.gameObject);
        ev.AddListener (M_TriggerEvent.EventType.OnTriggerEnter, () => {
            onTriggerZone = true;
        });
        ev.AddListener (M_TriggerEvent.EventType.OnTriggerExit, () => {
            onTriggerZone = false;
        });

    }

    private void StopGrab () {
        onGrab = false;
        pointForce.enabled = false;
    }

    private void Start () {
        Vector2 pointerP = Pointer.current.position.ReadValue ().ScreenToWold ();
        Vector2 centerP = Gb.charactorMain.position;

    }
    private void OnDisable () {
        StopGrab ();
    }

    private void FixedUpdate () {

        if (onGrab) {
            Vector2 pointerP = Pointer.current.position.ReadValue ().ScreenToWold ();
            Vector2 centerP = Gb.charactorMain.position;
            Vector2 target = default;


            bool pointerInsideRange = (pointerP - centerP).magnitude <= maxGrabDistance;
            if (pointerInsideRange) {
                target = pointerP;
            } else {
                target = (pointerP - centerP).normalized * maxGrabDistance + centerP;
                if ((rigidBody.position - centerP).magnitude > maxGrabDistance * 0.7f) {
                    Gb.charactorMain.MoveTo ((rigidBody.position + centerP) / 2, maxSpeed);
                }
            }

            pointForce.SetTarget (target);




            Fn.DrawLineOnScreen (centerP, rigidBody.position, 0.05f);
        }
    }

}