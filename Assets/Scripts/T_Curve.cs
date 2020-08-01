using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Curve : MonoBehaviour {
    public AnimationCurve curve;
    // Start is called before the first frame update


    public void AddPoint (float index, float value) {
        curve.AddKey (index, value);
    }
}