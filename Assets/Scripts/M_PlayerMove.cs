using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_PlayerMove : MonoBehaviour {
    [Header ("Dependent")]
    public Rigidbody2D rigidBody;
    [Header ("Setting")]
    public float force = 60f;
    public float maxSpeed = 6f;
    public AnimationCurve speedForceCurve = Fn.Curve.OneOneCurve;
    public float arriveDistance = 0.5f;
    public Input input = new Input ();
    public Test test = new Test ();
    [SerializeField, ReadOnly] private Vector2 gravity;
    [SerializeField, ReadOnly] private Component forceComp;
    [SerializeField, ReadOnly] private C_OnArrive onArriveComp;
    private EventTrigger inputEvent;


    private void OnEnable () {
        EnableInput ();

    }
    private void OnDisable () {
        DisableInput ();
    }


    //*Editor Event
#if UNITY_EDITOR
    private void Reset () {
        rigidBody = GetComponent<Rigidbody2D> ();
    }
    private void OnValidate () {

        if (enabled & Application.isPlaying) {
            UnityEditor.EditorApplication.delayCall += () => {

                if (test.stop) {
                    test.stop = false;
                    Stop ();
                }

                if (test.walk == 1) {
                    Move (Vector2.right);
                } else if (test.walk == -1) {
                    Move (Vector2.left);
                } else if (test.walk == 0) {
                    Stop ();
                }

                if (input.enable) {
                    EnableInput ();
                } else {
                    DisableInput ();
                }



            };

        }

    }
#endif



    //*Private Method
    private void DrawIndicate (Vector2 point) {
        Vector2 drawposition = point;
        RaycastHit2D raycastHit2D = Physics2D.Raycast (point, gravity, 3f, LayerMask.GetMask (input.groundLayer));
        if (raycastHit2D.collider) {
            drawposition = raycastHit2D.point - gravity.normalized;
        }
        GameObject circle = GameObject.Instantiate (input.indicateObject, drawposition, default);
        circle.Destroy (0.3f);
    }




    //* Public Method
    public Vector2 MoveTo (Vector2 position) {
        gravity = rigidBody.GetComponent<M_Gravity> ().GetGravity ();
        Vector2 point = position;
        Vector2 moveVector = (point - rigidBody.position).ProjectOnPlane (gravity);



        forceComp.Destroy ();
        forceComp = gameObject.Ex_AddTargetForce (position, force,
            speedForceCurve : speedForceCurve,
            curveMaxSpeed : maxSpeed,
            singleDimension : gravity.Rotate (90),
            createBy : this
        );


        Destroy (onArriveComp);
        onArriveComp = Fn._.OnArrive (rigidBody.gameObject, point, moveVector, arriveDistance, callBack : Stop);
        return point;
    }
    public void Move (Vector2 direction, float maxSpeed = -1f, float duration = -1f) {
        this.maxSpeed = (maxSpeed < 0) ? this.maxSpeed : maxSpeed;
        gravity = rigidBody.GetComponent<M_Gravity> ().GetGravity ();
        Vector2 moveVector = (direction).ProjectOnPlane (gravity);

        forceComp.Destroy ();
        forceComp = this.Ex_AddForce (moveVector.normalized * force, this.maxSpeed, speedForceCurve);

        if (duration > 0) forceComp.Destroy (duration);
    }
    public void Stop () {
        Destroy (forceComp);
        Destroy (onArriveComp);

    }
    public void EnableInput () {
        inputEvent = inputEvent?inputEvent : input.inputZone.Ex_AddInputToTrigger (
            EventTriggerType.PointerClick, (d) => {
                ////////////////////////////////////////
                PointerEventData data = (PointerEventData) d;
                Vector2 pointerP = data.position;

                Vector2 point = MoveTo (pointerP.ScreenToWold ());

                DrawIndicate (point);

            });
    }
    public void DisableInput () {
        inputEvent.Destroy ();
    }



    //*Property
    [System.Serializable]
    public class Test {
        public bool stop = false;
        [Range (-1, 1)] public int walk = 0;
    }

    [System.Serializable]
    public class Input {
        public bool enable = false;
        public Collider2D inputZone;
        public GameObject indicateObject;
        public string groundLayer = "Ground";
    }
}