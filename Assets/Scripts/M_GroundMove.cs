using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_GroundMove : MonoBehaviour {
    [Header ("Import")]
    public M_GroundNormalFinder importNormal;
    public Vector2 normal;
    public M_Gravity importGravity;
    public Vector2 gravity;
    public bool onGround;

    [Header ("Setting")]
    public float moveForce = 300f;
    public float moveForceOnAir = 150f;
    public float decelerate = 150f;
    public float decelIncRate = 10f;
    public Vector2 limitMaxSpeed = new Vector2 (20f, 20f);

    [Header ("Function")]
    public bool moveForceEnable;
    public bool gravityEnable;
    public bool decelerateOn;
    public bool cutUpsideSpeed;
    public bool limitMax;


    [Header ("Variables")]
    public WalkAction goDir;

    [Header ("Date For Test")]

    public Vector2 velosity;


    public Test test;


    private void FixedUpdate () {
        gravity = importGravity.GetGravity ();
        onGround = importNormal.IsOnGround ();
        normal = importNormal.GetGroundNormal ();
        normal = normal == Vector2.zero ? -gravity.normalized : normal;
        float walkDirection;
        switch (goDir) {
            default : walkDirection = 0;
            break;
            case WalkAction.left:
                    walkDirection = -1;
                break;
            case WalkAction.right:
                    walkDirection = 1;
                break;
        }


        Vector2 force = Vector2.zero;
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        Vector2 v = rb.velocity;


        if (gravityEnable) {
            Vector2 g = Vector3.Project (gravity, normal);
            force += g * rb.mass;
        }


        if (moveForceEnable) {
            Vector2 v_Hr = Vector3.ProjectOnPlane (v, normal);
            Vector2 v_Vr = v - v_Hr;

            Vector2 MoveDir = Fn.RotateClock (normal, 90).normalized;
            Vector2 forceAdd = walkDirection * MoveDir * moveForce;

            if (!onGround) {
                forceAdd = walkDirection * MoveDir * moveForceOnAir;
            }


            force += forceAdd;


        }


        if (decelerateOn) {
            Vector2 v_Hr = Vector3.ProjectOnPlane (v, normal);
            Vector2 v_Vr = v - v_Hr;


            Vector2 deForce = -1 * v_Hr.normalized * rb.mass *
                (decelerate + v_Hr.magnitude * decelIncRate);

            float deV = deForce.magnitude / rb.mass * Time.fixedDeltaTime;

            if (v_Hr.magnitude < deV) {
                deForce = -1 * v_Hr / Time.fixedDeltaTime * rb.mass;
            }

            force += deForce;


        }


        if (cutUpsideSpeed) {
            Vector2 v_Vr = Vector3.Project (v, normal);
            Vector2 v_Hr = v - v_Vr;
            bool isV_OnVrSide = v_Vr.normalized == normal.normalized;
            if (isV_OnVrSide) {
                v = v_Hr;
            }
        }

        if (limitMax) {
            Vector2 v_Hr = Vector3.ProjectOnPlane (v, normal);
            Vector2 v_Vr = v - v_Hr;

            v_Hr = v_Hr.magnitude > limitMaxSpeed.x?v_Hr.normalized * limitMaxSpeed.x : v_Hr;
            v_Vr = v_Vr.magnitude > limitMaxSpeed.x?v_Vr.normalized * limitMaxSpeed.y : v_Vr;


            v = v_Vr + v_Hr;
        }


        rb.AddForce (force);
        rb.velocity = v;

        velosity = v;
    }


   
    public void SetAction (WalkAction direction) {
        goDir = direction;
    }


    public enum WalkAction { left, stop, right }


    //*Test
    [System.Serializable]
    public class Test {
    
        [Range (-1, 1)]
        public int walk;


    }
    private void OnValidate () {
       

        switch (test.walk) {
            case -1:
                SetAction (WalkAction.left);
                break;
            case 0:
                SetAction (WalkAction.stop);
                break;
            case 1:
                SetAction (WalkAction.right);
                break;
        }

    }

}