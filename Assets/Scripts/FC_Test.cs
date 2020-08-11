using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FC_Core;

public class FC_Test : MonoBehaviour {
    public FC_Core core;

    public Rotate rotate;

    private void Awake () {

        core = core?core : GetComponent<FC_Core> ();

        core.AddModifier (rotate.CalcForce);

    }

    private void OnEnable () {
        core.AddModifier (rotate.CalcForce);
    }
    private void OnDisable () {
        core.RemoveModifier (rotate.CalcForce);
    }



    void Start () { }




    //*
    [System.Serializable]
    public class Rotate {
        public bool enable = true;
        public float targetAngle = 0;
        public float maxForce = 100;
        public AnimationCurve addCurve;
        public float mutiplyForce = 1f;
        public float maxSlowForce = 100;
        public float maxSlowAngle = 10;
        public AnimationCurve SlowCurve;
        public AnimationCurve SlowSpeedCurve;
        public float mutiplySlow = 1f;

        public void CalcForce (ForceModifier mod) {

            if (enable) {
                Rigidbody2D rb = mod.core.GetComponent<Rigidbody2D> ();
                float rotate = rb.rotation;

                rotate = rotate % 360;
                if (rotate > 180) rotate = rotate - 360;
                if (rotate < -180) rotate = 360 + rotate;


                float torque = 0;
                float delta = targetAngle - rotate;
                float delta_nor = delta / 180;

                float delta_remap = addCurve.Evaluate (Mathf.Abs (delta)) * Mathf.Sign (delta);
                torque = delta_remap * mutiplyForce * maxForce;


                float delta_remap_slow = SlowCurve.Evaluate (Mathf.Abs (delta)) * -Mathf.Sign (delta_nor);
                float speedmap = SlowSpeedCurve.Evaluate (Mathf.Abs (rb.angularVelocity));
                float torque2 = delta_remap_slow * mutiplySlow * maxSlowForce * speedmap;
                if (delta == 0) torque2 = 0;
                if (rb.angularVelocity * delta < 0) torque2 = 0;

                rb.AddTorque (torque * rb.mass);
                rb.AddTorque (torque2 * rb.mass);



                // rb.GetComponent<T_Curve> ().AddPoint (Time.time, rb.angularVelocity);


            }

        }
    }
}