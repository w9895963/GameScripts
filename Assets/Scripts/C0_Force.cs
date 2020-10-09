using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using UnityEngine;
using UnityEngine.Events;

public class C0_Force : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        [System.Serializable] public class Require {
            public Vector2 force;
        }
        public Require require = new Require ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;

            public SpeedForceCurve speedForceCurve = new SpeedForceCurve ();
            [System.Serializable] public class SpeedForceCurve {
                public bool enable = false;
                public AnimationCurve curve = Global.Curve.OneZero;
                public float maxSpeed = 5f;

                public void Set (bool enable, AnimationCurve curve = default, float maxSpeed = 0) {
                    this.enable = enable;
                    this.curve = curve;
                    this.maxSpeed = maxSpeed;
                }
            }

            public TargetDistanceScale targetDistanceScale = new TargetDistanceScale ();
            [System.Serializable] public class TargetDistanceScale {
                public bool enabled = false;
                public Vector2 target;
                public AnimationCurve curve = Curve.ZeroOne;
                public float maxDistance = 1;
                public float Scaler (Vector2 force, Vector2 position) {
                    Vector2 distVt = target - position;
                    float dist = distVt.magnitude;
                    return curve.Evaluate (dist, 0, maxDistance, 0, 1);
                }

            }

            public ForceCompensator forceCompensator = new ForceCompensator ();
            [System.Serializable] public class ForceCompensator {
                public bool enabled = false;
                public float targetSpeed;

            }
        }
        public Optional optional = new Optional ();

    }

    public Vector2 force;
    public Vector2 velosity;
    public Object createby;
    public PID pid;




    // *MAIN

    private void FixedUpdate () {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D> ();
        Vector2 position = rigidBody.position;
        Vector2 forceAdd = setting.require.force;




        if (setting.optional.speedForceCurve.enable) {
            var s = setting.optional.speedForceCurve;
            forceAdd *= s.curve.Evaluate (rigidBody.velocity.magnitude / s.maxSpeed);
        }

        if (setting.optional.targetDistanceScale.enabled) {
            forceAdd *= setting.optional.targetDistanceScale.Scaler (forceAdd, position);
        }


        if (setting.optional.ignoreMass)
            forceAdd *= rigidBody.mass;


        if (setting.optional.forceCompensator.enabled) {
            if (pid == null) {
                pid = new PID ();
                pid.I = 1;
                pid.max = 60;
                pid.min = 0;
            }
            var s = setting.optional.forceCompensator;
            Vector2 velocity = rigidBody.velocity;
            float currSpeed = velocity.magnitude;
            float targetSpeed = s.targetSpeed;
            float error = targetSpeed - velocity.x;
            float forceCalc = pid.Calc (error);
            forceAdd += forceCalc * Vector2.right;
        }

        rigidBody.AddForce (forceAdd);




        velosity = rigidBody.velocity;
        force = forceAdd;

    }




    //*Public Method
    public Vector2 Force {
        set {
            setting.require.force = value;
        }
        get {
            return setting.require.force;
        }
    }

    [System.Serializable] public class PID {
        public float I = 0.3f;
        public float I2 = 0.6f;
        public bool useLimit = false;
        public float max = 60;
        public float min = -60;
        private bool initial = false;
        private float integrate = 0;
        private float lastError;


        public float Calc (float error) {
            if (!initial) {
                lastError = error;
                initial = true;
            }
            float output;
            float delta = error - lastError;
            float wantDel = -error * I;
            float valueAdd = (wantDel - delta).Shape (1f, 1) * I2;
            integrate += -valueAdd;
            if (useLimit) integrate = integrate.Clamp (min, max);

            lastError = error;
            output = integrate;

            float index = Time.time * 20;

            return output;
        }
    }

}


public static class Extension_C_Force {
    public static C0_Force AddForce (this Rigid2dExMethod source, UnityAction<C0_Force.Setting> setProfile) {
        C0_Force comp = source.rigidbody.gameObject.AddComponent<C0_Force> ();
        comp.createby = source.callby;
        setProfile (comp.setting);
        return comp;
    }
    public static C0_Force AddForce (this Rigid2dExMethod source,
        Vector2 force,
        float maxSpeed = default,
        AnimationCurve speedCurve = default

    ) {

        C0_Force comp = source.rigidbody.gameObject.AddComponent<C0_Force> ();
        comp.createby = source.callby;

        comp.setting.require.force = force;
        if (maxSpeed != default) {
            var curve = comp.setting.optional.speedForceCurve;
            curve.enable = true;
            curve.maxSpeed = maxSpeed;
            if (speedCurve != default) {
                curve.curve = speedCurve;
            }
        }

        return comp;
    }

}