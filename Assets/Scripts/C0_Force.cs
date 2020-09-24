using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C0_Force : MonoBehaviour {
    [System.Serializable] public class Property {
        [System.Serializable] public class Require {
            public Rigidbody2D rigidBody = null;
            public Vector2 direction;
            public float force;
        }
        public Require require = new Require ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;
            public SpeedForceCurve speedForceCurve = new SpeedForceCurve ();
            public PointForceMode pointForceMode = new PointForceMode ();
        }
        public Optional optional = new Optional ();

    }
    public Property property = new Property ();

    public Object createby = null;




    // *MAIN
    private void FixedUpdate () {
        Rigidbody2D rigidBody = property.require.rigidBody;
        Vector2 forceAdd = property.require.force * property.require.direction.normalized;
        if (property.optional.speedForceCurve.enable) {
            forceAdd *= property.optional.speedForceCurve.curve.Evaluate (rigidBody.velocity.magnitude / property.optional.speedForceCurve.maxSpeed);
        }


        if (property.optional.ignoreMass) forceAdd *= rigidBody.mass;

        if (property.optional.pointForceMode.enable) {
            Vector2 position = rigidBody.transform.TransformPoint (property.optional.pointForceMode.localPosition);
            rigidBody.AddForceAtPosition (forceAdd, position);
        } else {
            rigidBody.AddForce (forceAdd);

        }
    }




    //*On
    private void OnValidate () {
        if (property.require.rigidBody == null) property.require.rigidBody = GetComponent<Rigidbody2D> ();
    }




    //* Property
    [System.Serializable]
    public class SpeedForceCurve {
        public bool enable = false;
        public AnimationCurve curve = Global.Curve.OneZeroCurve;
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
        this.property.require.force = force.magnitude;
        this.property.require.direction = force.normalized;
        this.property.optional.speedForceCurve.curve = forceCurve;
        this.property.optional.speedForceCurve.maxSpeed = maxSpeed;
        this.property.optional.speedForceCurve.enable = true;
    }
    public void SetPointForceMode (bool enable, Vector2 localPosition = default) {
        property.optional.pointForceMode.enable = enable;
        property.optional.pointForceMode.localPosition = localPosition;
    }
    public void SetSpeedForceCurve (bool enable, AnimationCurve curve = default, float maxSpeed = 0) {
        property.optional.speedForceCurve.Set (enable, curve, maxSpeed);
    }
    public static C0_Force AddForceComponent (GameObject gameObject, Component componentCall = null) {
        C0_Force comp = gameObject.AddComponent<C0_Force> ();
        comp.createby = componentCall;
        return comp;
    }
    public static C0_Force AddForce (GameObject gameObject, Vector2 force,
        float maxSpeed, AnimationCurve speedCurve, Component componentCall = null) {


        C0_Force comp = gameObject.AddComponent<C0_Force> ();
        comp.createby = componentCall;
        comp.SetForce (force, maxSpeed, speedCurve);
        return comp;
    }
}


public static class Extension_C_Force {
    public static C0_Force Ex_AddForce (this Component component,
            Vector2 force,
            float maxSpeed,
            AnimationCurve speedCurve) =>


        C0_Force.AddForce (component.gameObject, force, maxSpeed, speedCurve, component);


    public static C0_Force Ex_AddForce (this Component component) =>
        C0_Force.AddForceComponent (component.gameObject, component);
}