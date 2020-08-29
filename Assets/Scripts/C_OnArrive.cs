using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_OnArrive : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public Vector2 targetPosition;
    public float distance = 0.001f;
    public Vector2 distanceDirection = default;
    public UnityEvent onArrive = new UnityEvent ();

    private Vector2 lastPosition;
    private bool lastPositionSetup = false;




    private void FixedUpdate () {
        Main ();
    }

    private void Awake () {
        Initial ();
    }



    //*Privat Method
    private void Main () {
        if (lastPositionSetup) {



            if (distanceDirection == default) {
                Vector2 v1 = targetPosition - lastPosition;
                Vector2 v2 = targetPosition - rigidBody.position;
                Vector2 dir = rigidBody.position - lastPosition;
                Vector2 projPoint = v1.Project (dir) + lastPosition;

                float closedDist = v1.magnitude.Min (v2.magnitude);

                bool isOnLine = false;
                float moveDist = dir.magnitude;
                if ((projPoint - lastPosition).magnitude < moveDist
                    & (projPoint - rigidBody.position).magnitude < moveDist) {
                    isOnLine = true;
                }
                if (isOnLine) {
                    closedDist = (projPoint - targetPosition).magnitude;
                }
                if (closedDist <= distance) {
                    onArrive.Invoke ();
                }

            } else {

                Vector2 pLast = Vector3.Project (lastPosition, distanceDirection);
                Vector2 pNow = Vector3.Project (rigidBody.position, distanceDirection);
                Vector2 tar = Vector3.Project (targetPosition, distanceDirection);
                Vector2 vLast = tar - pLast;
                Vector2 vNow = tar - pNow;


                float closedDist = vNow.magnitude.Min (vLast.magnitude);
                float moveDist = (pNow - pLast).magnitude;
                if ((tar - pLast).magnitude < moveDist
                    & (tar - pNow).magnitude < moveDist) {

                    closedDist = 0;
                }

                if (closedDist <= distance) {
                    onArrive.Invoke ();
                }

            }
        } else {
            Vector2 dir = rigidBody.position - targetPosition;
            if (distanceDirection != default) {
                dir = dir.Project (dir);
            }
            if (dir.magnitude <= distance) {
                onArrive.Invoke ();
            }
        }

        lastPosition = rigidBody.position;
        lastPositionSetup = true;
    }
    private void Initial () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }


    //*Public Method
    public void SetPosition (Vector2 position, Vector2 distanceDirection = default, float distance = 0.01f) {
        targetPosition = position;
        this.distanceDirection = distanceDirection;
        this.distance = distance;
    }
    public void SetEvent (UnityAction action, bool AutoDestroy = true) {
        onArrive.AddListener (action);
        if (AutoDestroy) onArrive.AddListener (() => Destroy (this));
    }


}
public static class _Extension_M_OnArrive {
    public static C_OnArrive OnArrive (this Fn fn, GameObject gameObject,
        Vector2 position, Vector2 distanceDirection = default,
        float distance = 0.01f, bool autoDestroy = true,
        UnityAction callBack = null) {


        C_OnArrive comp = gameObject.AddComponent<C_OnArrive> ();
        comp.SetPosition (position, distanceDirection, distance);
        comp.SetEvent (callBack, autoDestroy);

        return comp;
    }
}