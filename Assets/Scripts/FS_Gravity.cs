using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_Gravity : MonoBehaviour {
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private bool ignoreMass = true;
    [SerializeField]
    private Vector2 gravity = new Vector2 (0, -30);




    private void OnValidate () {
        if (rigidbody == null) { rigidbody = GetComponent<Rigidbody2D> (); }
    }


    private void FixedUpdate () {
        Vector2 force = gravity;
        force *= (ignoreMass == true) ? rigidbody.mass : 1;
        rigidbody.AddForce (force);
    }
}