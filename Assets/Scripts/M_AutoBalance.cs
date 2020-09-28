using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.U2D;

public class M_AutoBalance : MonoBehaviour {
    public float V = 0.1f;
    public int V1 = 5;
    public List<AnimationCurve> cv = new List<AnimationCurve> ();
    public Rigidbody2D rigid;
    public float target = 0;
    public float input;
    public float currV;
    public float output;
    private float lastOutput;

    private void FixedUpdate () {
        rigid = GetComponent<Rigidbody2D> ();
        var lastInput = input;
        input = target - rigid.velocity.y;
        currV = input - lastInput;

        float wantV = -input;
        float delV = wantV - currV;
        var outputV = output - lastOutput;
        lastOutput = output;

        output = output + input * V + currV * V1 ;
        rigid.AddForce (Vector2.up * output);


        VisibalCurve.AddKey (0, Time.time, input / 2);
        VisibalCurve.AddKey (1, Time.time, currV);
        VisibalCurve.AddKey (2, Time.time, output / 10);
    }

    private void Update () {


    }
    private void Start () {
        VisibalCurve.Create (Color.red, new Vector2 (0, -40));
        VisibalCurve.Create (Color.yellow, new Vector2 (0, -40));
        VisibalCurve.Create (Color.green, new Vector2 (0, -40));


        output = 1;

    }
}