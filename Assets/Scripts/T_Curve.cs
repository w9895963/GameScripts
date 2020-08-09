using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Curve : MonoBehaviour {
    public AnimationCurve curve;
    public Vector2 vlast;
    public float angularV;
    // Start is called before the first frame update


    private void FixedUpdate () {

        float angularVelocity = GetComponent<Rigidbody2D> ().angularVelocity;
        AddPoint (Time.time, (angularVelocity - angularV) / Time.fixedDeltaTime);
        angularV = angularVelocity;
        // vlast = GetComponent<Rigidbody2D> ().velocity;
    }
    public void AddPoint (float index, float value) {
        curve.AddKey (index, value);
    }
}