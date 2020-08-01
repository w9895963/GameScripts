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

    private void Awake () {
        importGravity.events.gravityChanged.AddListener (OnGravityChanged);
        // importGravity.events.gravityChanged.RemoveListener (OnGravityChanged);

    }


    private void Update () {
        gravity = importGravity.GetGravity ();

        if (gravityChanged) {
            float z = transform.rotation.z;
            float angle = Vector2.SignedAngle (Vector2.down, gravity);
            float delTime = Time.time - gravityChangedTime;


            delTime = delTime > totalTime?totalTime : delTime;
            float rate = delTime / totalTime;
            rate = curve.Evaluate (rate);


            z = angle * rate;
            Quaternion quaternion = Quaternion.Euler (0, 0, z);

            transform.rotation = quaternion;

            // Debug.Log (transform.rotation.z + ";" + angle + ";" + rate + ";" + z);
            bool changedFinished = transform.rotation.z == angle;
            if (changedFinished) {
                gravityChanged = false;
            }
        }

    }


    public void OnGravityChanged () {
        gravityChangedTime = Time.time;
        gravityChanged = true;
    }


}