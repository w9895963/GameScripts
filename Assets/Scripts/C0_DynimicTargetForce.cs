using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class C0_DynimicTargetForce : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 target;
            public float maxForce;
        }
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;
            public PIDv2.Setting pidSetting = new PIDv2.Setting ();

            public VelosityControl velosityControl = new VelosityControl ();
            [System.Serializable] public class VelosityControl {
                public bool enabled = false;
                public float maxSpeed = 5;
                public float minSpeed = 0.1f;
                public float slowDownDistance = 1;
                public AnimationCurve slowDownCurve = Curve.ZeroOne;


            }
            public SingleDimension singleDimension = new SingleDimension ();
            [System.Serializable] public class SingleDimension {
                public bool enabled = false;
                public Vector2 dimensiion;
            }
        }
    }

    public float speed;
    public Object creator;
    public string lable;
    // * ---------------------------------- 
    private PIDv2 pid2 = new PIDv2 ();

    private void Awake () {
        pid2.setting = setting.optional.pidSetting;
        pid2.max.enabled = true;
    }

    private void FixedUpdate () {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
        Vector2 position = rigidbody.position;
        Vector2 velocity = rigidbody.velocity;

        Vector2 target = setting.require.target;
        float maxForce = setting.require.maxForce;

        Vector2 forceAdd = Vector2.zero;


        if (setting.optional.singleDimension.enabled) {
            SingleDimensionProcess (position, ref target);
        }

        Vector2 targetV;
        if (setting.optional.velosityControl.enabled) {
            Vector2 distVt = target - position;
            var s = setting.optional.velosityControl;
            float mag = s.slowDownCurve.Evaluate (distVt.magnitude, 0, s.slowDownDistance, s.minSpeed, s.maxSpeed);
            targetV = distVt.normalized * mag;
        } else {
            Vector2 distVt = target - position;
            targetV = distVt;
        }

        pid2.max.max = setting.require.maxForce;

        forceAdd += pid2.CalcOutput (targetV - velocity);




        if (setting.optional.ignoreMass) {
            forceAdd *= rigidbody.mass;
        }

        rigidbody.AddForce (forceAdd);



        speed = velocity.magnitude;


    }
    private void SingleDimensionProcess (Vector2 position, ref Vector2 target) {
        var s = setting.optional.singleDimension;
        Vector2 distVt = target - position;
        target = distVt.Project (s.dimensiion) + position;
    }

    [System.Serializable] public class PIDv2 {
        public Setting setting = new Setting ();
        [System.Serializable] public class Setting {
            public float deltaRate = 0.3f;
            public float changedRate = 45f;
        }
        public Max max = new Max ();
        [System.Serializable] public class Max {
            public bool enabled = false;
            public float max = 60;
        }
        private bool initial = false;
        private Vector2 integrate = default;
        private Vector2 lastError;


        public Vector2 CalcOutput (Vector2 error) {
            var s = setting;
            if (!initial) {
                lastError = error;
                initial = true;
            }
            Vector2 output;
            Vector2 delta = error - lastError;
            Vector2 wantDel = -error * s.deltaRate;
            Vector2 valueAdd = (wantDel - delta) * s.changedRate;
            integrate += -valueAdd;
            if (max.enabled) integrate = integrate.ClampMax (max.max);

            lastError = error;
            output = integrate;

            float index = Time.time * 20;

            return output;
        }
    }




}