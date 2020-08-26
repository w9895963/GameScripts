﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class C1_TargetForce : MonoBehaviour {

    [SerializeField] private Rigidbody2D rigidBody = null;
    [SerializeField] private bool ignoreMass = true;
    [SerializeField] private Vector2 targetPosition = Vector2.zero;
    [SerializeField] private float force = 40f;
    //* Plugin
    [SerializeField] private ForceCurve distanceForceCurve = new ForceCurve ();
    [SerializeField] private C_Force.SpeedForceCurve speedForceCurve = new C_Force.SpeedForceCurve ();
    [SerializeField] private PointForce forceAtpoint = new PointForce ();
    [SerializeField] private UseObjectTarget useObjectTarget = new UseObjectTarget ();
    [SerializeField] private SingleDimension singleDimension = new SingleDimension ();
    [SerializeField, ReadOnly] private Vector2 forceAdd = Vector2.zero;
    [SerializeField, ReadOnly] private C_Force forceComp = null;
    [ReadOnly] public Component createBy = null;




    // *Private Method
    private void Main () {
        if (useObjectTarget.enable) targetPosition = useObjectTarget.target.transform.position;


        Vector2 disVector = targetPosition - rigidBody.position;
        Vector2 dimension = singleDimension.dimension;
        disVector = dimension == default ? disVector : disVector.Project (dimension);

        float forceRate = 1f;
        if (distanceForceCurve.enable) {
            forceRate = distanceForceCurve.distanceCurve.Evaluate (disVector.magnitude / distanceForceCurve.maxDistance);
        }
        forceAdd = disVector.normalized * force * forceRate;

        // if (ignoreMass) forceAdd *= rigidBody.mass;



        UpdataDate ();
        forceComp.Force = forceAdd;
    }


    private void UpdataDate () {
        if (forceComp) {
            forceComp.SetPointForceMode (forceAtpoint.enable, forceAtpoint.localPosition);
            forceComp.SetSpeedForceCurve (speedForceCurve.enable, speedForceCurve.curve, speedForceCurve.maxSpeed);
            forceComp.IgnoreMass = ignoreMass;
        }

    }
    private void Setup () {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody2D> ();
        if (forceComp == null) forceComp = this.AddForceComponent ();
    }

    //*Main
    private void FixedUpdate () {
        Main ();
    }

    //*Event
    private void OnValidate () {
        UpdataDate ();

    }

    private void Awake () {
        Setup ();


    }

    private void OnEnable () {
        Setup ();

    }
    private void OnDisable () {
        Destroy (forceComp);
    }




    //*Public Method
    public void SetTarget (Vector2 position) {
        targetPosition = position;
    }
    public void SetForce (float force) {
        this.force = force;
    }
    public void SetForceDistanceCurve (AnimationCurve curve, float maxDistance) {
        distanceForceCurve.enable = true;
        distanceForceCurve.distanceCurve = curve;
        distanceForceCurve.maxDistance = maxDistance;
        UpdataDate ();
    }

    public static C1_TargetForce AddTargetForce (GameObject gameObject, Vector2 targetPosition,
        float force,
        Vector2 applyPosition = default,
        AnimationCurve forceCurve = null,
        float curveDistance = 1f) {


        C1_TargetForce comp = gameObject.AddComponent<C1_TargetForce> ();
        comp.force = force;
        if (applyPosition != Vector2.zero) {
            comp.forceAtpoint.enable = true;
            comp.forceAtpoint.localPosition = applyPosition;
        }
        if (forceCurve != null) {
            comp.distanceForceCurve.enable = true;
            comp.distanceForceCurve.distanceCurve = forceCurve;
            comp.distanceForceCurve.maxDistance = curveDistance;
        }


        return comp;
    }


    //*Property Class
    [System.Serializable]
    public class UseObjectTarget {
        public bool enable = false;
        public GameObject target;
    }

    [System.Serializable]
    public class PointForce {
        public bool enable = false;
        public Vector2 localPosition = default;
    }

    [System.Serializable]
    public class SingleDimension {
        public bool enable = false;
        public Vector2 dimension = default;
    }

    [System.Serializable]
    public class ForceCurve {
        public bool enable = false;
        public AnimationCurve distanceCurve = Fn.Curve.ZeroOneCurve;
        public float maxDistance = 1f;
    }




}


public static class _Extension_FS_PointForce {
    public static C1_TargetForce Ex_AddTargetForce (this Component component,
        Vector2 targetPosition,
        float force,
        Vector2 applyPosition = default,
        AnimationCurve forceCurve = null,
        float curveMaxDistance = 1f) {



        C1_TargetForce ctargetForce = C1_TargetForce.AddTargetForce (component.gameObject,
            targetPosition,
            force,
            applyPosition,
            forceCurve,
            curveMaxDistance);



        ctargetForce.createBy = component;
        return ctargetForce;
    }

}