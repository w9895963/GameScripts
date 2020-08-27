using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_Force : MonoBehaviour {

    [SerializeField] private Rigidbody2D rigidBody = null;
    public bool ignoreMass = true;
    public Vector2 force = Vector2.zero;
    [SerializeField] private SpeedForceCurve speedForceCurve = new SpeedForceCurve ();
    [SerializeField] private PointForceMode pointForceMode = new PointForceMode ();
    [SerializeField, ReadOnly] private Component createby = null;




    // *MAIN
    private void FixedUpdate () {
        MainUpdate ();
    }

    private void MainUpdate () {
        Vector2 forceAdd = force;
        if (speedForceCurve.enable) {
            forceAdd *= speedForceCurve.curve.Evaluate (rigidBody.velocity.magnitude / speedForceCurve.maxSpeed);
        }


        if (ignoreMass) forceAdd *= rigidBody.mass;

        if (pointForceMode.enable) {
            Vector2 position = rigidBody.transform.TransformPoint (pointForceMode.localPosition);
            rigidBody.AddForceAtPosition (forceAdd, position);
        } else {
            rigidBody.AddForce (forceAdd);
        }
    }




    //*On
    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }
    private void Awake () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }



    //* Property
    [System.Serializable]
    public class SpeedForceCurve {
        public bool enable = false;
        public AnimationCurve curve = Fn.Curve.OneZeroCurve;
        public float maxSpeed = 5f;

        public void Set (bool enable, AnimationCurve curve = default, float maxSpeed = 0) {
            this.enable = enable;
            this.curve = curve;
            this.maxSpeed = maxSpeed;
        }
    }

    [System.Serializable]
    public class PointForceMode {
        public bool enable = false;
        public Vector2 localPosition = Vector2.zero;
    }


    //*Public Method


    public void SetForce (Vector2 force, float maxSpeed, AnimationCurve forceCurve) {
        this.force = force;
        this.speedForceCurve.curve = forceCurve;
        this.speedForceCurve.maxSpeed = maxSpeed;
        this.speedForceCurve.enable = true;
    }
    public void SetPointForceMode (bool enable, Vector2 localPosition = default) {
        pointForceMode.enable = enable;
        pointForceMode.localPosition = localPosition;
    }
    public void SetSpeedForceCurve (bool enable, AnimationCurve curve = default, float maxSpeed = 0) {
        speedForceCurve.Set (enable, curve, maxSpeed);
    }
    public static C_Force AddForceComponent (GameObject gameObject, Component componentCall = null) {
        C_Force comp = gameObject.AddComponent<C_Force> ();
        comp.createby = componentCall;
        return comp;
    }
    public static C_Force AddForce (GameObject gameObject, Vector2 force,
        float maxSpeed, AnimationCurve speedCurve, Component componentCall = null) {


        C_Force comp = gameObject.AddComponent<C_Force> ();
        comp.createby = componentCall;
        comp.SetForce (force, maxSpeed, speedCurve);
        return comp;
    }
}


public static class _Extension_C_Force {
    public static C_Force Ex_AddForce (this Component component,
            Vector2 force,
            float maxSpeed,
            AnimationCurve speedCurve) =>


        C_Force.AddForce (component.gameObject, force, maxSpeed, speedCurve, component);


    public static C_Force Ex_AddForce (this Component component) =>
        C_Force.AddForceComponent (component.gameObject, component);
}