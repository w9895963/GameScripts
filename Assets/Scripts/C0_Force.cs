using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using UnityEngine;
using UnityEngine.Events;

public class C0_Force : MonoBehaviour {
    public Property property = new Property ();
    [System.Serializable] public class Property {
        [System.Serializable] public class Require {
            public Rigidbody2D rigidBody = null;

            public SimpleForce simpleForce = new SimpleForce ();
            [System.Serializable] public class SimpleForce {
                public bool enabled = false;
                public Vector2 force;

            }

            public TargetForceMode targetForceMode = new TargetForceMode ();
            [System.Serializable] public class TargetForceMode {
                public bool enabled = false;
                public GameObject target;
                public float force = 5;
                public bool useDistanceCurve = false;
                public float maxDistance = 1;
                public AnimationCurve distanceCurve = Curve.Default;
                public bool useAccruedScore = false;
                public float accruedScoreScale = 1;
            }
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

            public PointForceMode pointForceMode = new PointForceMode ();
            [System.Serializable] public class PointForceMode {
                public bool enable = false;
                public Vector2 localPosition = Vector2.zero;
            }

            public SlowDownForce slowDownForce = new SlowDownForce ();
            [System.Serializable] public class SlowDownForce {
                public bool enabled = false;
                public GameObject target;
                public float maxDistance = 1f;
                public float maxSpeed = 40;
                public AnimationCurve speedControlCurve = Curve.ZeroOne;
                public AnimationCurve slowdownForceCurve = Curve.OneZero;
                public bool useAccruedScore = false;
                public float accruedScoreScale = 1;

            }


        }
        public Optional optional = new Optional ();

    }


    public DynimicForce targetVelosityMode = new DynimicForce ();


    public Object createby = null;
    private Vector2 inte = Vector2.zero;
    private float accruedScore = 0;
    public Vector2 velocity;




    // *MAIN
    private void FixedUpdate () {
        Rigidbody2D rigidBody = property.require.rigidBody;
        Vector2 forceAdd = default;

        if (property.require.simpleForce.enabled) {
            Property.Require.SimpleForce s = property.require.simpleForce;
            forceAdd += s.force;
        }
        if (property.require.targetForceMode.enabled) {
            Property.Require.TargetForceMode s = property.require.targetForceMode;
            Vector2 v = s.target.Get2dPosition () - rigidBody.position;
            Vector2 currV = rigidBody.velocity;
            float dist = v.magnitude;
            float targetV = (dist * 3).ClampMax (1);
            float scale = 1;
            if (s.useDistanceCurve) {
                scale = s.distanceCurve.Evaluate (dist / s.maxDistance);
            }
            forceAdd += s.force * v.normalized * scale;

            forceAdd += v.normalized * accruedScore;

            if (s.useAccruedScore) {
                if (currV.magnitude < targetV) {

                    accruedScore += s.accruedScoreScale / currV.magnitude.ClampMin (0.2f) * dist.ClampMax (1).Pow (2);
                } else {
                    accruedScore -= s.accruedScoreScale * 60 / currV.magnitude.ClampMin (0.2f) * dist.ClampMax (1).Pow (2);
                    accruedScore = accruedScore.ClampMin (0);
                }
            } else {
                accruedScore = 0;
            }

        }

        //*optional
        if (property.optional.speedForceCurve.enable) {
            Property.Optional.SpeedForceCurve s = property.optional.speedForceCurve;
            forceAdd *= s.curve.Evaluate (rigidBody.velocity.magnitude / s.maxSpeed);
        }


        if (property.optional.slowDownForce.enabled) {
            Property.Optional.SlowDownForce s = property.optional.slowDownForce;
            Vector2 tarP = s.target.Get2dPosition ();
            Vector2 vc = tarP - rigidBody.position;
            float dist = vc.magnitude;
            Vector2 currV = rigidBody.velocity;
            float speedScale = s.speedControlCurve.Evaluate (dist / s.maxDistance);
            float slowdownForceScale = s.slowdownForceCurve.Evaluate (dist / s.maxDistance);
            Vector2 targetV = (speedScale * s.maxSpeed) * vc.normalized;
            Vector2 currV_H = currV.Project (targetV);
            Vector2 currV_V = currV.ProjectOnPlane (targetV);


            var acc_V = -currV_V / Time.fixedDeltaTime * slowdownForceScale;
            var acc_H = (targetV - currV_H) / Time.fixedDeltaTime * slowdownForceScale;
            if (vc.IsSameSide (currV_H) & vc.IsSameSide (acc_H)) {
                acc_H = Vector2.zero;
            }



            forceAdd += acc_V;
            forceAdd += acc_H;

        }



        if (property.optional.ignoreMass) forceAdd *= rigidBody.mass;

        if (property.optional.pointForceMode.enable) {
            Vector2 position = rigidBody.transform.TransformPoint (property.optional.pointForceMode.localPosition);
            rigidBody.AddForceAtPosition (forceAdd, position);
        } else {
            rigidBody.AddForce (forceAdd);
        }


        rigidBody.AddForce (targetVelosityMode.GetForce (rigidBody.velocity));
        velocity = rigidBody.velocity;

    }




    //*On
    private void OnValidate () {
        if (property.require.rigidBody == null) property.require.rigidBody = GetComponent<Rigidbody2D> ();
    }




    //*Public Method


    public void SetForce (Vector2 force, float maxSpeed, AnimationCurve forceCurve) {
        this.property.require.simpleForce.force = force;
        this.property.require.simpleForce.enabled = true;
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

namespace Global {
    [System.Serializable] public class DynimicForce {
        public Vector2 targetVelosity = default;

        public Advance advance = new Advance ();
        [System.Serializable] public class Advance {
            public float velosityChangedRate = 1f;
            public bool calcEnvironmentForce = true;
            public float updateRate = 0.9f;
        }
        public Vector2 fixedForce;
        private Vector2 currVelosity;
        private Vector2 lastForce;
        public ErrorElimate errorfixed = new ErrorElimate ();
        public Vector2 GetForce (Vector2 currentVelosity, float targetForce = -1) {
            Vector2 lastV = currVelosity;
            currVelosity = currentVelosity;
            Vector2 currV = currVelosity;
            Vector2 lastF = lastForce;
            Vector2 targetV = targetVelosity;
            Vector2 dtV = targetV - currV;
            Vector2 acc = dtV / Time.fixedDeltaTime;
            Vector2 force1 = default;
            // force1 = acc * advance.velosityChangedRate;
            Vector2 force2 = Vector2.zero;


            if (advance.calcEnvironmentForce) {
                Vector2 realDV = currV - lastV;
                Vector2 realForce = realDV / Time.fixedDeltaTime;
                Vector2 outForce = realForce - lastF;

                float rate = advance.updateRate;

                float lastError = (targetV.y - lastV.y);
                float currError = (targetV.y - currV.y);
                float lastDtEr = currError - lastError;
                float wantDtEr = currError * 1;
                float dtdtEr = wantDtEr - lastDtEr;
                float output = dtdtEr * 2;

                fixedForce += Vector2.up * output;

                // force2 = fixedForce;

                force2 = Vector2.up * errorfixed.Calc (targetV.y - currV.y);
            }




            if (targetForce > 0) {
                force1 = force1.ClamMax (targetForce);
            }

            lastForce = force1 + force2;

            return lastForce;

        }


    }

    [System.Serializable] public class ErrorElimate {

        public float currError = 0;
        public float lastDtEr;
        public float wantDtEr;
        public float addUp = 0;
        public float scale = 1;
        public float dtdtEr;

        public float Calc (float error) {
            float lastError = currError;
            currError = error;
            lastDtEr = currError - lastError;
            wantDtEr = ((currError.Abs () + 1).Pow (0.5f) - 1) * currError.Sign () * 1;
            dtdtEr = wantDtEr - lastDtEr;


            if (error >= 0) {
                if (dtdtEr >= 0) {
                    addUp += dtdtEr * scale;
                } else {
                    float p = -dtdtEr + 1;
                    float v = -((p.Pow (1) - 1));
                    addUp += v * scale;
                }
            }




            return addUp;
        }
    }

}