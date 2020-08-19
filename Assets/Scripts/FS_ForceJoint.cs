using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_ForceJoint : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D fromObject = null;
    [SerializeField]
    private Rigidbody2D toObject;
    [SerializeField]
    private MapVariable mapping = new MapVariable ();
    [SerializeField]
    private AnimationCurve forceCurve = Fn.Curve.defautCurve;
    [SerializeField]
    private float maxForce = 30;
    [SerializeField]
    private float maxTorque = 200;
    [SerializeField, ReadOnly]
    private float forceValue;

    private void FixedUpdate () {

        //float difRotation = fromObject.rotation - lastRotationFr;

        Vector2 direction = mapping.positionEnd - mapping.positionStart;
        Vector2 positionOnRoad = Vector3.Project (toObject.position - mapping.positionStart, direction);
        float positionRate = positionOnRoad.magnitude / direction.magnitude;
        // positionRate = Mathf.Clamp01 (positionRate);
        float rotationRate = (fromObject.rotation - mapping.rotationStart)
            / (mapping.rotationEnd - mapping.rotationStart);
        // rotationRate = Mathf.Clamp01 (rotationRate);
        float diffOnRate = positionRate - rotationRate;


        Vector2 dir = direction.normalized;
        forceValue = forceCurve.Evaluate (Mathf.Abs (diffOnRate)) * -Mathf.Sign (diffOnRate);

        // float index=Fn.Curve();



        toObject.AddForce (forceValue * dir * maxForce);
        fromObject.AddTorque (-forceValue * maxTorque);

        // if (lastPositionTo == toObject.position) {
        //     fromObject.rotation = lastRotationFr;
        // }



        // lastPositionTo = toObject.position;
        // lastRotationFr = fromObject.rotation;

    }

    private void OnValidate () {
        toObject = (toObject == null) ? GetComponent<Rigidbody2D> () : toObject;

    }

    [System.Serializable]
    public class MapVariable {
        public float rotationStart;
        public float rotationEnd;
        public Vector2 positionStart;
        public Vector2 positionEnd;
    }
}