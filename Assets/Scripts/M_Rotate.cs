using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Rotate : MonoBehaviour {

    [Header ("Import")]
    public M_Gravity importGravity;
    public Vector2 gravity;
    [Header ("Setting")]
    public float totalTime = 0.7f;
    public AnimationCurve curve;
    [Header ("Trigger")]
    public bool gravityChanged = false;

    [Header ("Data")]
    public float gravityChangedTime;
    public Vector2 standDirection;
    public float angleToRotate;

    private void Awake () {
        importGravity?.events.onGravityChange.AddListener (OnGravityChanged);

    }


    private void Update () {
        gravity = importGravity.GetGravity ();

        if (gravityChanged) {
            var z = Vector2.SignedAngle (Vector2.up, standDirection);


            float delTime = Time.time - gravityChangedTime;


            delTime = delTime > totalTime?totalTime : delTime;
            float rate = delTime / totalTime;
            rate = curve.Evaluate (rate);

            float delZ = angleToRotate * rate;
            Quaternion quaternion = Quaternion.Euler (0, 0, z + delZ);

            transform.rotation = quaternion;

            bool changedFinished = Time.time - gravityChangedTime > totalTime;
            if (changedFinished) {
                gravityChanged = false;
            }
        }

    }


    public void OnGravityChanged () {
        gravity = importGravity.GetGravity ();
        gravityChangedTime = Time.time;

        standDirection = Fn.RotateClock (Vector2.up, -transform.rotation.eulerAngles.z);
        angleToRotate = Vector2.SignedAngle (standDirection, -gravity);
        gravityChanged = true;
    }


}