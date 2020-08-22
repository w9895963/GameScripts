using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FS_PointForce : MonoBehaviour {

    public Rigidbody2D rigidBody;
    public bool ignoreMass = true;
    public Vector2 targetPosition;
    public float force = 40f;
    public AnimationCurve forceCurve = Fn.Curve.ZeroOneCurve;
    public float curveToDistance = 1f;
    public Vector2 distacneOnDirection = default;
    public UseTarget useTarget;
    public ArriveEvents onArrive;

    //
    [SerializeField, ReadOnly]
    private Vector2 forceAdd;
    private Vector2 lastPosition;
    private bool lastPositionSetup = false;




    // *MAIN
    private void FixedUpdate () {
        Main ();

        ArriveEvent ();

        lastPosition = rigidBody.position;
        lastPositionSetup = true;
    }

    private void ArriveEvent () {
        if (lastPositionSetup & lastPosition != rigidBody.position) {

            if (onArrive.distanceDirection == default) {

                Vector2 v1 = targetPosition - lastPosition;
                Vector2 v2 = targetPosition - rigidBody.position;
                Vector2 closedP = (Vector2) Vector3.Project (v1, rigidBody.position - lastPosition) + lastPosition;


                bool isClosePointOnLine = false;
                float moveDist = (rigidBody.position - lastPosition).magnitude;
                if ((closedP - lastPosition).magnitude < moveDist
                    & (closedP - rigidBody.position).magnitude < moveDist) {

                    isClosePointOnLine = true;
                }

                if (v1.magnitude > onArrive.distance) {
                    if (v2.magnitude <= onArrive.distance) {
                        onArrive.arrive.Invoke ();
                    } else if (isClosePointOnLine
                        & (closedP - targetPosition).magnitude <= onArrive.distance) {
                        onArrive.arrive.Invoke ();
                    }

                }

            } else {

                Vector2 pLast = Vector3.Project (lastPosition, onArrive.distanceDirection);
                Vector2 pNow = Vector3.Project (rigidBody.position, onArrive.distanceDirection);
                Vector2 tar = Vector3.Project (targetPosition, onArrive.distanceDirection);
                Vector2 vLast = tar - pLast;
                Vector2 vNow = tar - pNow;


                bool isTargetOnLine = false;
                float moveDist = (pNow - pLast).magnitude;

                if ((tar - pLast).magnitude < moveDist
                    & (tar - pNow).magnitude < moveDist) {

                    isTargetOnLine = true;
                }

                if (vLast.magnitude > onArrive.distance) {
                    if (vNow.magnitude <= onArrive.distance) {
                        onArrive.arrive.Invoke ();
                    } else if (isTargetOnLine) {
                        onArrive.arrive.Invoke ();
                    }

                }
            }


        }
    }

    private void Main () {
        if (useTarget.enable) targetPosition = useTarget.target.transform.position;


        Vector2 disVector = targetPosition - rigidBody.position;
        disVector = distacneOnDirection == default ? disVector : disVector.Project (distacneOnDirection);
        float forceRate = forceCurve.Evaluate (disVector.magnitude / curveToDistance);
        forceAdd = disVector.normalized * force * forceRate;
        if (ignoreMass) forceAdd *= rigidBody.mass;


        rigidBody.AddForce (forceAdd);
    }

    //*Public Method
    public void SetPoint (Vector2 position, UnityAction arriveCallback = default) {
        targetPosition = position;
        if (arriveCallback != default) {
            onArrive.arrive.AddListener (arriveCallback);
        }

    }
    public void RemoveCallback (UnityAction callback) {
        onArrive.arrive.RemoveListener (callback);
    }




    //*OnValidate
    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }




    //*Property Class
    [System.Serializable]
    public class UseTarget {
        public bool enable;
        public GameObject target;
    }

    [System.Serializable]
    public class ArriveEvents {
        public float distance = 0.001f;
        public Vector2 distanceDirection = default;
        public UnityEvent arrive = new UnityEvent ();


    }
}