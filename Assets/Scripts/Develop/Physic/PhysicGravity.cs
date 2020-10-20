using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Physic;
using UnityEngine;
using static Global.Physic.PhysicUtility;




[RequireComponent (typeof (Rigidbody2D), typeof (PhysicForceAction))]
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

    private void Awake () {
        gravity = setting.gravity;
    }
    private void OnEnable () {
        AddPhysicAction (gameObject, 0, GravityAction);
    }

    private void OnDisable () {
        RemovePhysicAction (gameObject, GravityAction);
    }
    private void OnValidate () {
        gravity = setting.gravity;
    }
    //*  Fields
    private Vector2 gravity;

    //*  Property

    //*  Method
    private void GravityAction (ActionData data) {
        Vector2 force = gravity * GetComponent<Rigidbody2D> ().mass;
        data.SetForce (force);
    }
    //*  Class
    [System.Serializable] public class Setting {
        public Vector2 gravity;
    }

}