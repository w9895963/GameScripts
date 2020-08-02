using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static M_GroundMove;

public class MP_GroundMovePlugin : MonoBehaviour {
    [Header ("Import")]
    public M_GroundMove importGroundMove;
    public M_Gravity importGravity;
    public Vector2 gravity;
    public M_InputFilter importInputControl;
    [Header ("Setting")]
    public float minDistance = 0.7f;
    [Header ("Status")]
    public bool onWalking;

    [Header ("Date")]
    public Vector2 destination;
    public float distance;

    public Test test;

    private void Awake () {
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        importInputControl = importInputControl?importInputControl : GetComponent<M_InputFilter> ();
        importInputControl.events.click.AddListener (() => {

            Vector2 dest = importInputControl.GetGroundPosition ();
            Vector2 v = Vector3.Project (importInputControl.GetTargetInner () - rb.position, gravity);
            Vector2 v2 = Vector3.ProjectOnPlane (importInputControl.GetTargetInner () - rb.position, gravity);


            bool succeed = false;
            if (dest != default) {
                WalkTo (dest);
                succeed = true;

            } else if (Mathf.Abs (v.y) < 6) {
                dest = rb.position + v2 + gravity.normalized * 0.5f;
                WalkTo (dest);
                succeed = true;

            }

            if (succeed) {
                D_DebugEventAction.CreateSign (dest, Vector2.SignedAngle (Vector2.down, gravity));
            }


        });
    }

    private void FixedUpdate () {
        if (onWalking) {
            gravity = importGravity.GetGravity ();


            Vector2 position = importGroundMove.GetComponent<Rigidbody2D> ().position;
            Vector2 distanceV = destination - position;
            distance = Vector3.ProjectOnPlane (distanceV, gravity).magnitude;


            if (distance < minDistance) {
                importGroundMove.SetAction (WalkAction.stop);
                onWalking = false;
            } else {
                onWalking = true;
            }
        }


    }


    public void WalkTo (Vector2 dest) {

        gravity = importGravity.GetGravity ();

        var positionStart = importGroundMove.GetComponent<Rigidbody2D> ().position;


        Vector2 moveVector = dest - positionStart;
        Vector2 mvVtOn_X = Vector3.ProjectOnPlane (moveVector, gravity);
        float distanceToMove = mvVtOn_X.magnitude;
        Vector2 rightDir_nrml = Fn.RotateClock (-gravity, 90).normalized;
        float angle = Vector2.Angle (mvVtOn_X, rightDir_nrml);
        WalkAction action = angle == 0 ? WalkAction.right : WalkAction.left;


        if (distanceToMove > minDistance) {
            importGroundMove.SetAction (action);
            destination = dest;
            onWalking = true;
        }

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