using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RotationFriction : MonoBehaviour {
    public bool ignoreMass = true;
    public float multiplier = 0.2f;
    public float maxTorque = 360;
    public float maxAngle = 5;
    [SerializeField, ReadOnly] private bool moving = false;
    [SerializeField, ReadOnly] private float baseRotate;
    private bool baseRotateSetup = false;

    void Start () {

    }

    void Update () {

    }

    private void OnDisable () {
        baseRotateSetup = false;
    }

    void FixedUpdate () {
        Rigidbody2D body = GetComponent<Rigidbody2D> ();
        var rotate = body.rotation;

        if ((baseRotate - rotate).Abs () > maxAngle) {
            moving = true;
            baseRotate = (baseRotate - rotate).Clamp (-maxAngle, maxAngle) + rotate;
        }

        if (moving & (baseRotate - rotate).Abs () < maxAngle) {
            baseRotate = rotate;
            moving = false;
        }



        if (baseRotateSetup) {
            float torque = ((baseRotate - rotate) * multiplier).Clamp (-maxTorque, maxTorque);
            if (ignoreMass) torque *= body.mass;
            body.AddTorque (torque);
        }




        if (!baseRotateSetup) baseRotate = rotate;
        baseRotateSetup = true;
    }
}