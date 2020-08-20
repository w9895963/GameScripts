using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_SnapToPoint : MonoBehaviour {

    public Rigidbody2D rigidBody;
    public bool ignoreMass = true;
    public bool useObjectPosition;
    public GameObject target;
    public Vector2 position;
    public float distance = 1f;
    public float force = 40f;
    public AnimationCurve forceCurve = Fn.Curve.DefautCurve;




    void Start () {

    }

    private void FixedUpdate () {
        if (useObjectPosition) position = target.transform.position;


        if ((rigidBody.position - position).magnitude < distance) { 
            Vector2 disVector = position - rigidBody.position;
            float forceRate = forceCurve.Evaluate (disVector.magnitude / distance);
            Vector2 forceAdd = disVector.normalized * force * forceRate;
            // if (ignoreMass) forceAdd *= rigidBody.mass;
            rigidBody.AddForce (forceAdd);
        }
    }


    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }
}