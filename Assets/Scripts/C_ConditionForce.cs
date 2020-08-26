using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ConditionForce : MonoBehaviour {
    public Rigidbody2D targetRigidbody;
    public Rigidbody2D fromRigidbody;
    public float force = 0;
    public Curve forceCurve = new Curve ();
    public float torque = 0f;
    public Curve torqueCurve = new Curve ();
    public Condition fromCondition = new Condition ();
    public Condition targetCondition = new Condition ();
    private C1_TargetForce targetForceComp = null;

    void FixedUpdate () {
        float targetRate = targetCondition.CalcRate (targetRigidbody);
        float fromRate = fromCondition.CalcRate (fromRigidbody);



        float delRate = targetRate - fromRate;
        float disntance = targetCondition.GetDisntance (targetRigidbody.position);
        Vector2 vector = targetCondition.GetVector ();
        float curveResult = 1;
        if (forceCurve.enable)
            curveResult = forceCurve.curve.Evaluate (disntance / forceCurve.indexMax);
        Vector2 addForce =
            delRate.Sign ()
            * force
            * vector.normalized
            * curveResult;
        // targetRigidbody.AddForce (addForce);


        Vector2 targetP = targetCondition.CalcPosition (fromRate);
        if (targetForceComp == null) {
            targetForceComp = this.Ex_AddTargetForce (targetP, addForce.magnitude);
        } else {
            targetForceComp.SetTarget (targetP);
            targetForceComp.SetForce (addForce.magnitude);
        }

    }



    void OnValidate () {
        UpdataDate ();
    }
    void Awake () {
        SetUp ();
    }

    private void OnDisable () {
        if (targetCondition != null)
            Destroy (targetForceComp);
    }




    //*Private Method
    private void SetUp () {
        targetRigidbody = targetRigidbody ? targetRigidbody : GetComponent<Rigidbody2D> ();
    }
    private void UpdataDate () {
        if (forceCurve.enable)
            targetForceComp?.SetForceDistanceCurve (forceCurve.curve, forceCurve.indexMax);
    }




    //*Property Class
    [System.Serializable]
    public class Condition {
        public CalculateProperty property = CalculateProperty.Position;
        public Vector2 positionBegin = Vector2.zero;
        public Vector2 positionEnd = Vector2.one;
        public float rotationBegin = 0;
        public float rotationEnd = 360;

        public float CalcRate (Rigidbody2D rigidbody) {
            float targetRate = 0;
            if (property == CalculateProperty.Position) {
                Vector2 dirV = positionEnd - positionBegin;
                Vector2 currP = rigidbody.position;
                Vector2 prjDir = (currP - positionBegin).Project (dirV);

                Quaternion quaternion = Quaternion.FromToRotation (dirV, Vector2.right);
                prjDir = quaternion * prjDir;
                dirV = quaternion * dirV;
                // targetRate = prjDir.magnitude / (dirV.magnitude);
                // if ((dirV.normalized + prjDir.normalized).magnitude < 1) {
                //     targetRate *= -1;
                // }
                targetRate = prjDir.x / dirV.x;
            } else if (property == CalculateProperty.Rotation) {
                float currR = rigidbody.rotation;
                targetRate = (currR - rotationBegin) / (rotationEnd - rotationBegin);
            }

            return targetRate;
        }
        public Vector2 CalcPosition (float rate) {
            return (positionEnd - positionBegin).normalized * rate + positionBegin;
        }
        public float CalcRotation (float rate) {
            return (rotationEnd - rotationBegin) * rate + rotationBegin;
        }


        public float GetDisntance (Vector2 currentPosition) {
            Vector2 dirV = positionEnd - positionBegin;
            Vector2 prjPosition = currentPosition.Project (dirV);
            float distance = (prjPosition - positionBegin).magnitude;
            return distance;
        }
        public Vector2 GetVector () {
            return positionEnd - positionBegin;
        }
    }

    [System.Serializable]
    public class Result {
        public bool enable = false;
        public Vector2 positionBegin = Vector2.zero;
        public Vector2 positionEnd = Vector2.one;
        public float rotationBegin = 0;
        public float rotationEnd = 360;


    }

    [System.Serializable]
    public class Curve {
        public bool enable = false;
        public float indexMax = 1;
        public AnimationCurve curve = Fn.Curve.OneOneCurve;
    }

    [System.Serializable]
    public enum CalculateProperty { Position, Rotation }
}