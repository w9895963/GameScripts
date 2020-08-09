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


    void Start () { }




    //*
    [System.Serializable]
    public class Rotate {
        public bool enable = true;


        public void CalcForce (ForceModifier mod) {

            if (enable) {
                Rigidbody2D rb = mod.core.GetComponent<Rigidbody2D> ();

             //   rb.AddTorque (100 * Mathf.Deg2Rad * rb.inertia);


                // rb.GetComponent<T_Curve> ().AddPoint (Time.time, rb.angularVelocity);


            }

        }
    }
}