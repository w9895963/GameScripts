using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Global.Physic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent (typeof (Rigidbody2D))]
public class PhysicManager : MonoBehaviour {
    public List < (int, UnityAction<PhysicAction>) > ActionList => actionList;
    // *-------Private-------Private-------Private------- 
    private List < (int, UnityAction<PhysicAction>) > actionList = new List < (int, UnityAction<PhysicAction>) > ();

    private PhysicAction actionData;

    private void Awake () {
        var gravities = gameObject.GetComponentsInChildren<IGravity> ();
        gravities.ForEach ((gravity) => {
            PhysicUtility.AddPhysicAction (gameObject, PhysicOrder.Gravity, (action) => {
                action.SetForce (gravity.Gravity);
            });
        });
    }

    void FixedUpdate () {
        actionData = new PhysicAction ();
        actionData.CurrentIndex = 0;
        actionList.ForEach ((x) => {
            var action = x.Item2;
            actionData.CurrentIndex = x.Item1;
            action (actionData);
            actionData.CurrentIndex++;
        });
        gameObject.GetComponent<Rigidbody2D> ().AddForce (actionData.GetTotalForce ());
    }
}