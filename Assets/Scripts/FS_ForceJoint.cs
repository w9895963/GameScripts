using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_ForceJoint : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D rotationObject = null;
    [SerializeField]
    private Rigidbody2D moveObject;
    [SerializeField]
    private MapVariable mapping = new MapVariable ();
    [SerializeField]
    private AnimationCurve forceCurve = Fn.Curve.ZeroOneCurve;
    [SerializeField]
    private float maxForce = 30;
    [SerializeField]
    private float maxTorque = 200;
    [SerializeField, ReadOnly]
    private float forceValue;

    private void FixedUpdate () {


        Vector2 direction = mapping.positionEnd - mapping.positionStart;
        Vector2 positionOnRoad = Vector3.Project (moveObject.position - mapping.positionStart, direction);
        float positionRate = positionOnRoad.magnitude / direction.magnitude;
        float rotationRate = (rotationObject.rotation - mapping.rotationStart)
            / (mapping.rotationEnd - mapping.rotationStart);
        float diffOnRate = positionRate - rotationRate;


        Vector2 dir = direction.normalized;
        forceValue = forceCurve.Evaluate (Mathf.Abs (diffOnRate)) * -Mathf.Sign (diffOnRate);




        moveObject.AddForce (forceValue * dir * maxForce);
        rotationObject.AddTorque (-forceValue * maxTorque);


    }

    private void OnValidate () {
        moveObject = (moveObject == null) ? GetComponent<Rigidbody2D> () : moveObject;

    }

    [System.Serializable]
    public class MapVariable {
        public float rotationStart;
        public float rotationEnd;
        public Vector2 positionStart;
        public Vector2 positionEnd;
    }


    //*Method
    public void UpDateRotation () {
        Vector2 v1 = moveObject.position - mapping.positionStart;
        Vector2 v2 = mapping.positionEnd - mapping.positionStart;

        Quaternion quaternion = Quaternion.AngleAxis (Vector2.SignedAngle (v2, Vector2.right), Vector3.forward);
        v2 = quaternion * v2;
        v1 = quaternion * v1;
        // float rate = v1.magnitude / v2.magnitude * Mathf.Sign (Vector2.Angle (v1, v2));
        // float roLength = mapping.rotationEnd - mapping.rotationStart;
        float rate = v1.x / v2.x;
        float roLength = mapping.rotationEnd - mapping.rotationStart;

        float r_S = roLength * rate + rotationObject.rotation;
        float r_E = r_S + roLength;
        mapping.rotationStart = r_S;
        mapping.rotationEnd = r_E;


    }
}