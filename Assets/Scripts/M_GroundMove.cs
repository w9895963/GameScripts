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

    [Header ("Setting")]
    public float maxSpeed = 20f;
    public float fallMax = 20f;
    public float accelerate = 100f;
    public float decelerate = 60f;

    [Header ("Function")]
    public bool moveForce;
    public bool fallForce;
    public bool decelerateOn;
    public bool cutUpsideSpeed;


    [Header ("Control Port")]
    public float walkDirection;

    [Header ("Date")]

    public Vector2 velosity;


    public Test test;


    private void FixedUpdate () {
        gravity = importGravity.GetGravity ();
        normal = importNormal.GetGroundNormal ();
        normal = normal == Vector2.zero? - gravity.normalized : normal;

        Vector2 force = Vector2.zero;
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        Vector2 v = rb.velocity;


        if (fallForce) {
            Vector2 g = Vector3.Project (gravity, normal);
            force += g;
        }


        if (moveForce) {
            Vector2 v_Hr = Vector3.ProjectOnPlane (v, normal);
            Vector2 v_Vr = v - v_Hr;
            if (v_Hr.magnitude < maxSpeed) {
                Vector2 HorDir = Fn.RotateClock (normal, 90).normalized;
                force += walkDirection * HorDir * accelerate * rb.mass;
            } else {
                v = v_Hr.normalized * maxSpeed + v_Vr;
            }
        }


        if (decelerateOn) {
            Vector2 v_Hr = Vector3.ProjectOnPlane (v, normal);
            Vector2 v_Vr = v - v_Hr;
            bool isCurrSpeedBigger = v_Hr.magnitude > decelerate * Time.fixedDeltaTime;

            if (isCurrSpeedBigger) {
                force -= v_Hr.normalized * decelerate;
            } else {
                v = v_Vr;
            }
        }

        if (cutUpsideSpeed) {
            Vector2 v_Vr = Vector3.Project (v, normal);
            Vector2 v_Hr = v - v_Vr;
            bool isV_OnVrSide = v_Vr.normalized == normal.normalized;
            if (isV_OnVrSide) {
                v = v_Hr;
            }
        }


        rb.AddForce (force);
        rb.velocity = v;

        velosity = v;
    }


    public void SetWalkingDirection (float direction) {
        walkDirection = direction;
    }


    //*Test
    [System.Serializable]
    public class Test {
        public bool walkLeft;
        public bool walkRight;
        public bool stop;

    }
    private void OnValidate () {
        if (test.walkLeft) {
            walkDirection = -1;
        }
        if (test.walkRight) {
            walkDirection = 1;
        }
        if (test.stop) {
            walkDirection = 0;

        }
        test.walkLeft = false;
        test.walkRight = false;
        test.stop = false;

    }

}