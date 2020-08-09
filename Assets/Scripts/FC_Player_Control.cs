using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FC_Player_Control : MonoBehaviour {
    [Header ("Import")]
    public FC_Player importPlayer;
    public M_Gravity importGravity;
    public Vector2 gravity;
    public M_InputFilter importInputControl;
    [Header ("Setting")]
    public float minDistance = 0.7f;
    public float VerticalDistanceClick = 4f;
    public GameObject sign;
    [Header ("Status")]
    public bool onWalking;

    [Header ("Date")]
    public Vector2 destination;
    public float distance;
    [Header ("Event")]
    public Events events;

    public Test test;

    private void Awake () {
        importInputControl = importInputControl?importInputControl : GetComponent<M_InputFilter> ();
        importInputControl.events.click.AddListener (() => {

            if (enabled) {

                Rigidbody2D rb = GetComponent<Rigidbody2D> ();
                gravity = importGravity.GetGravity ();

                Vector2 dest = importInputControl.GetClickPosition ();
                Vector2 dest_vector = dest - rb.position;
                Vector2 dest_Vr = Vector3.Project (dest_vector, gravity);


                bool verticleDistCheck = dest_Vr.magnitude < VerticalDistanceClick;
                if (verticleDistCheck) {
                    WalkTo (dest);
                    CreateSign (dest);
                }

            }


        });
    }

    private void CreateSign (Vector2 dest) {
        GameObject obj = Fn.Create (sign, dest, Vector2.Angle (Vector2.up, -gravity));
        M_Gravity childG = obj.GetComponentInChildren<M_Gravity> ();
        childG.SetGravityDirection (gravity);
        // childG.gameObject.GetComponent<Rigidbody2D> ().velocity = gravity.normalized * 40;
        childG.gameObject.GetComponent<Rigidbody2D> ().AddForce (gravity.normalized * 40, ForceMode2D.Impulse);
        Fn.AddOneTimeListener (new UnityEvent[] { events.onArrived, importInputControl.events.click },
            () => {
                Destroy (obj);
            });
    }


    private void FixedUpdate () {
        if (onWalking) {
            gravity = importGravity.GetGravity ();

            Vector2 position = importPlayer.GetComponent<Rigidbody2D> ().position;
            Vector2 distanceV = destination - position;
            distance = Vector3.ProjectOnPlane (distanceV, gravity).magnitude;


            if (distance < minDistance) {
                importPlayer.Stop ();
                events.onArrived.Invoke ();

                onWalking = false;
            } else {
                onWalking = true;
            }
        }


    }


    public void WalkTo (Vector2 dest) {

        gravity = importGravity.GetGravity ();

        // var positionStart = importGroundMove.GetComponent<Rigidbody2D> ().position;
        var positionStart = importPlayer.GetComponent<Rigidbody2D> ().position;


        Vector2 moveVector = dest - positionStart;
        Vector2 mvVtOn_X = Vector3.ProjectOnPlane (moveVector, gravity);
        float distanceToMove = mvVtOn_X.magnitude;
        Vector2 rightDir_nrml = Fn.RotateClock (-gravity, 90).normalized;
        float angle = Vector2.Angle (mvVtOn_X, rightDir_nrml);
        // WalkAction action = angle == 0 ? WalkAction.right : WalkAction.left;


        if (distanceToMove > minDistance) {
            // importGroundMove.SetAction (action);
            if (angle == 0) importPlayer.Walk (1);
            else importPlayer.Walk (-1);
            destination = dest;
            onWalking = true;
        }

    }

    public void Stop () {
        onWalking = false;
    }

    [System.Serializable]
    public class Events {
        public UnityEvent onArrived;
    }


    //* test

    [System.Serializable]
    public class Test {
        public bool walkTo;
        public Vector2 position;

    }
    private void OnValidate () {
        if (test.walkTo == true) { WalkTo (test.position); }
        test.walkTo = false;
    }


}