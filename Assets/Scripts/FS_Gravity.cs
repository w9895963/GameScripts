using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FS_Gravity : MonoBehaviour {
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private bool ignoreMass = true;
    [SerializeField]
    private Vector2 gravity = new Vector2 (0, -30);
    public Events events;
    public Debug debug;



    //*Main
    private void FixedUpdate () {
        Vector2 force = gravity;
        force *= (ignoreMass == true) ? rigidbody.mass : 1;
        rigidbody.AddForce (force);
    }




    //*Method
    public void SetGravityDirection (float gravityAngle) {
        gravity = Quaternion.AngleAxis (gravityAngle, Vector3.forward) * (gravity.magnitude * Vector3.right);
        events.onGravityChange.Invoke ();
    }
    public void SetGravityDirection (Vector2 vector) {
        gravity = gravity.magnitude * vector.normalized;
        events.onGravityChange.Invoke ();
    }
    public void SetGravityAngle (Vector2 direction) {
        gravity = gravity.magnitude * direction.normalized;
        events.onGravityChange.Invoke ();
    }
    public void SetGravit (Vector2 gravity) {
        this.gravity = gravity;
        events.onGravityChange.Invoke ();
    }
    public Vector2 GetGravity () => gravity;



    //*OnValidate
    private void OnValidate () {
        if (rigidbody == null) { rigidbody = GetComponent<Rigidbody2D> (); }

        if (debug.setGravityAngle) {
            debug.setGravityAngle = false;
            SetGravityDirection (debug.angle);
        }

    }




    //*Debug
    [System.Serializable]
    public class Debug {
        public bool setGravityAngle;
        public float angle;
    }
    //*Property
    [System.Serializable]
    public class Events {
        public UnityEvent onGravityChange;
    }
}