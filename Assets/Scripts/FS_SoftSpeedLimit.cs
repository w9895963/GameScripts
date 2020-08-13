using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtensionMethod;

public class FS_SoftSpeedLimit : MonoBehaviour {
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private bool ignoreMass = true;
    [SerializeField]
    private Fn.Curve SpeedToResistForceCurve = new Fn.Curve ();
    [SerializeField, ReadOnly]
    private Vector2 forceAdd;
    [SerializeField, ReadOnly]
    private float speed;


    private void FixedUpdate () {
        Vector2 velosity = rigidbody.velocity;
        speed = velosity.magnitude;
        forceAdd = SpeedToResistForceCurve.Evaluate (speed) * -velosity.normalized;
        forceAdd *= (ignoreMass == true) ? rigidbody.mass : 1;



        rigidbody.AddForce (forceAdd);
    }


    private void OnValidate () {
        if (rigidbody == null) { rigidbody = GetComponent<Rigidbody2D> (); }
    }


}