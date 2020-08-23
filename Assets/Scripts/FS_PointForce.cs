using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FS_PointForce : MonoBehaviour {

    [SerializeField] private Rigidbody2D rigidBody = null;
    [SerializeField] private bool ignoreMass = true;
    [SerializeField] private bool pointForceMode = false;
    [SerializeField] private Vector2 pointForcePosition = default;
    [SerializeField] private Vector2 targetPosition = default;
    [SerializeField] private float force = 40f;
    [SerializeField] private AnimationCurve forceCurve = Fn.Curve.ZeroOneCurve;
    [SerializeField] private float curveToDistance = 1f;
    [SerializeField] private Vector2 calculateDirection = default;
    [SerializeField] private UseObjectTarget useObjectTarget = new UseObjectTarget ();
    [SerializeField] private Property property = new Property ();

    //
    [SerializeField, ReadOnly] private Vector2 forceAdd;




    // *MAIN
    private void FixedUpdate () {
        Main ();

    }


    private void Main () {
        if (useObjectTarget.enable) targetPosition = useObjectTarget.target.transform.position;


        Vector2 disVector = targetPosition - rigidBody.position;
        disVector = calculateDirection == default ? disVector : disVector.Project (calculateDirection);
        float forceRate = forceCurve.Evaluate (disVector.magnitude / curveToDistance);
        forceAdd = disVector.normalized * force * forceRate;
        if (ignoreMass) forceAdd *= rigidBody.mass;

        if (pointForceMode) {
            rigidBody.AddForceAtPosition (forceAdd, rigidBody.GetRelativePoint (pointForcePosition));
        } else {
            rigidBody.AddForce (forceAdd);
        }
    }




    //*On
    private void Awake () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
    }
    private void OnValidate () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();

    }



    //*Public Method
    public void SetTarget (Vector2 position) {
        targetPosition = position;
    }
    public void SetTargetOnSelf () {
        targetPosition = rigidBody.position;
    }
    public void SetUp (Property pt) {
        ignoreMass = pt.ignoreMass;
        pointForceMode = pt.pointForceMode;
        pointForcePosition = pt.pointForcePosition;
        force = pt.force;
        forceCurve = pt.forceCurve;
        curveToDistance = pt.curveToDistance;
        calculateDirection = pt.calculateDirection;
        useObjectTarget.enable = pt.useObjectTarget.enable;
        useObjectTarget.target = pt.useObjectTarget.target;

    }


    //*Property Class
    [System.Serializable]
    public class UseObjectTarget {
        public bool enable;
        public GameObject target;
    }

    [System.Serializable]
    public class Property {
        public bool ignoreMass = true;
        public bool pointForceMode = false;
        public Vector2 pointForcePosition = default;
        public float force = 40f;
        public AnimationCurve forceCurve = Fn.Curve.ZeroOneCurve;
        public float curveToDistance = 1f;
        public Vector2 calculateDirection = default;
        public UseObjectTarget useObjectTarget = new UseObjectTarget ();
    }


}