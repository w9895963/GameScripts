﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_OnArrive : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public Vector2 targetPosition;
    public float distance = 0.001f;
    public Vector2 distanceDirection = default;
    public UnityEvent onArrive = new UnityEvent ();

    private Vector2 lastPosition;
    private bool lastPositionSetup = false;



    private void Awake () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }
    private void FixedUpdate () {
        ArriveEvent ();
    }


    private void ArriveEvent () {
        if (lastPositionSetup & lastPosition != rigidBody.position) {

            if (distanceDirection == default) {

                Vector2 v1 = targetPosition - lastPosition;
                Vector2 v2 = targetPosition - rigidBody.position;
                Vector2 closedP = (Vector2) Vector3.Project (v1, rigidBody.position - lastPosition) + lastPosition;


                bool isClosePointOnLine = false;
                float moveDist = (rigidBody.position - lastPosition).magnitude;
                if ((closedP - lastPosition).magnitude < moveDist
                    & (closedP - rigidBody.position).magnitude < moveDist) {

                    isClosePointOnLine = true;
                }

                if (v1.magnitude > distance) {
                    if (v2.magnitude <= distance) {
                        onArrive.Invoke ();
                    } else if (isClosePointOnLine
                        & (closedP - targetPosition).magnitude <= distance) {
                        onArrive.Invoke ();
                    }

                }

            } else {

                Vector2 pLast = Vector3.Project (lastPosition, distanceDirection);
                Vector2 pNow = Vector3.Project (rigidBody.position, distanceDirection);
                Vector2 tar = Vector3.Project (targetPosition, distanceDirection);
                Vector2 vLast = tar - pLast;
                Vector2 vNow = tar - pNow;


                bool isTargetOnLine = false;
                float moveDist = (pNow - pLast).magnitude;

                if ((tar - pLast).magnitude < moveDist
                    & (tar - pNow).magnitude < moveDist) {

                    isTargetOnLine = true;
                }

                if (vLast.magnitude > distance) {
                    if (vNow.magnitude <= distance) {
                        onArrive.Invoke ();
                    } else if (isTargetOnLine) {
                        onArrive.Invoke ();
                    }

                }
            }


        }

        lastPosition = rigidBody.position;
        lastPositionSetup = true;
    }




    //*OnValidate
    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }

    //*Method
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