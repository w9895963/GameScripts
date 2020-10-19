using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
[RequireComponent (typeof (Rigidbody2D), typeof (PhysicForce))]
public class PhysicGravity : MonoBehaviour, IGravity {
    //*  Public Fields
    public Setting setting = new Setting ();


    //*  Public Property
    public Vector2 Gravity {
        get =>
            gravity;
        set =>
            gravity = value;
    }


    //*  Public Method
    public void ResetGravity () {
        gravity = setting.gravity;
    }
    // *-------Private-------Private-------Private------- 
    //*  Basic Event 
    private void Awake () {
        gravity = setting.gravity;
    }
    private void OnEnable () {
        GetComponent<PhysicForce> ().AddPhysicAction (0, GravityAction);
    }


    private void OnDisable () {
        GetComponent<PhysicForce> ().RemovePhysicAction (GravityAction);
    }
    private void OnValidate () {
        gravity = setting.gravity;
    }
    //*  Fields
    private Vector2 gravity;

    //*  Property

    //*  Method
    private void GravityAction (PhysicForce.ActionData data) {
        data.addForce = gravity * GetComponent<Rigidbody2D> ().mass;
    }
    //*  Class
    [System.Serializable] public class Setting {
        public Vector2 gravity;
    }

}