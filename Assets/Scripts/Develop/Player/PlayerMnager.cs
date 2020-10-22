using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Physic;
using static Global.Physic.PhysicUtility;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMnager : MonoBehaviour, ILayer, IGravity, IPlayer {
    public Vector2 gravity = new Vector2 (0, -40);
    public float moveForce = 80;
    public float maxSpeed = 8;
    public float jumpForce = 200f;
    public float jumpLastTime;
    public AnimationCurve jumpCurve = Curve.OneZero;
    public int LayerIndex => LayerUtility.Lead.Index;
    public Vector2 Gravity => gravity;
    private Vector2 moveButton;
    private float jumpButton;
    private Jump jump;

    private void Awake () {

        InputUtility.MoveInput.performed += (d) => {
            moveButton = d.ReadValue<Vector2> ();
        };
        InputUtility.JumpInput.performed += (d) => {
            jumpButton = d.ReadValue<float> ();
        };


    }

    private void OnEnable () {
        InputUtility.MoveInput.Enable ();
        InputUtility.JumpInput.Enable ();
        AddPhysicAction (gameObject, PhysicOrder.Movement, WalkAction);
        AddPhysicAction (gameObject, PhysicOrder.Jump, JumpAction);


    }
    private void OnDisable () {
        InputUtility.MoveInput.Disable ();
        InputUtility.JumpInput.Disable ();
        RemovePhysicAction (gameObject, WalkAction);
        RemovePhysicAction (gameObject, JumpAction);
    }

    private void WalkAction (PhysicAction action) {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();
        Vector2 velosity = rigidbody.velocity;
        float scaler = Curve.Evaluate (velosity.x.Abs (), maxSpeed * 0.5f, maxSpeed, 1, 0);
        action.SetForce (new Vector2 (moveButton.x, 0) * moveForce * scaler);
    }

    private void JumpAction (PhysicAction action) {
        if (jump == null) {
            if (jumpButton > 0) {
                jump = new Jump ();
                jump.beginTime = Time.time;
                jump.jumpCurve = jumpCurve;
                jump.jumpLastTime = jumpLastTime;
            }
        } else {
            if (Time.time - jump.beginTime > jump.jumpLastTime) {
                jump = null;
            }
        }



        if (jump != null) {
            action.SetForce (new Vector2 (0, jump.JumpScaler) * jumpForce);
        }

    }


    private class Jump {
        public float beginTime;
        public AnimationCurve jumpCurve;
        public float jumpLastTime;
        public float JumpScaler {
            get {
                float v = jumpCurve.Evaluate ((Time.time - beginTime) / jumpLastTime);
                Debug.Log (v);
                return v;
            }
        }
    }
}