using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Curve : MonoBehaviour {
    public AnimationCurve curve;
    public Vector2 vlast;
    // Start is called before the first frame update


    private void FixedUpdate () {

        AddPoint (Time.time, (GetComponent<Rigidbody2D> ().velocity - vlast).magnitude / Time.fixedDeltaTime);
        vlast = GetComponent<Rigidbody2D> ().velocity;
    }
    public void AddPoint (float index, float value) {
        curve.AddKey (index, value);
    }
}