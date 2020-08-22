using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FS_SimpleForce : MonoBehaviour {

    public Rigidbody2D rigidBody;
    public bool ignoreMass = true;
    public Vector2 force;
    public SpeedForceCurve speedForceCurve = new SpeedForceCurve ();




    // *MAIN
    private void FixedUpdate () {

        Vector2 forceAdd = force;
        if (speedForceCurve.enable) {
            forceAdd *= speedForceCurve.curve.Evaluate (rigidBody.velocity.magnitude / speedForceCurve.maxSpeed);
        }


        if (ignoreMass) forceAdd *= rigidBody.mass;
        rigidBody.AddForce (forceAdd);
    }




    //*OnValidate
    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }



    //* Property
    [System.Serializable]
    public class SpeedForceCurve {
        public bool enable = false;
        public AnimationCurve curve = Fn.Curve.OneOneCurve;
        public float maxSpeed = 5f;
    }


    //*Method
    public void SetForce (Vector2 force) {
        this.force = force;
        this.speedForceCurve.enable = false;
    }
    public void SetForce (Vector2 force, float maxSpeed, AnimationCurve forceCurve) {
        this.force = force;
        this.speedForceCurve.curve = forceCurve;
        this.speedForceCurve.maxSpeed = maxSpeed;
        this.speedForceCurve.enable = true;
    }

}