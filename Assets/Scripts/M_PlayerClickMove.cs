using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class M_PlayerClickMove : MonoBehaviour {
    [Header ("Dependent")]
    public Collider2D inputZone;
    public Rigidbody2D rigidBody;
    [Header ("Setting")]
    public float force = 60f;
    public float maxSpeed = 6f;
    public AnimationCurve forceCurve = Fn.Curve.OneOneCurve;
    public float arriveDistance = 0.5f;
    public GameObject indicateObject;
    public string groundLayer = "Ground";
    public Debug debug = new Debug ();
    [SerializeField, ReadOnly] private Vector2 gravity;
    [SerializeField, ReadOnly] private C_Force simpleForce;
    [SerializeField, ReadOnly] private C_OnArrive onArrive;

    void Start () {
        Fn._.AddEventToTrigger (inputZone.gameObject, EventTriggerType.PointerClick, (d) => {
            if (enabled) {
                PointerEventData data = (PointerEventData) d;
                Vector2 pointerP = data.position;

                Vector2 point = MoveTo (pointerP.ScreenToWold (), maxSpeed);

                DrawIndicate (point);

            }
        });


    }

    private void DrawIndicate (Vector2 point) {
        Vector2 drawposition = point;
        RaycastHit2D raycastHit2D = Physics2D.Raycast (point, gravity, 3f, LayerMask.GetMask (groundLayer));
        if (raycastHit2D.collider) {
            drawposition = raycastHit2D.point - gravity.normalized;
        }
        GameObject circle = GameObject.Instantiate (indicateObject, drawposition, default);
        Fn.WaitToCall (1, () => Destroy (circle));
    }



    public Vector2 MoveTo (Vector2 position, float maxSpeed) {
        gravity = rigidBody.GetComponent<FS_Gravity> ().GetGravity ();
        if (simpleForce == null) {
            simpleForce = rigidBody.gameObject.AddComponent<C_Force> ();
        }


        Vector2 point = position;
        Vector2 moveVector = (point - rigidBody.position).ProjectOnPlane (gravity);

        simpleForce.SetForce (moveVector.normalized * force, maxSpeed, forceCurve);
        simpleForce.enabled = true;


        if (onArrive) Destroy (onArrive);
        onArrive = Fn._.OnArrive (rigidBody.gameObject, point, () => simpleForce.enabled = false, moveVector, arriveDistance);
        return point;
    }
    public void Stop () {
        Destroy (simpleForce);
        Destroy (onArrive);

    }

    //*OnValidate
    private void OnValidate () {
        inputZone = inputZone? inputZone : GetComponent<Collider2D> ();
        rigidBody = rigidBody?rigidBody : GetComponent<Rigidbody2D> ();


        if (debug.stop) {
            debug.stop = false;
            Stop ();
        }

    }



    //*Debug
    [System.Serializable]
    public class Debug {
        public bool stop = false;
    }
}