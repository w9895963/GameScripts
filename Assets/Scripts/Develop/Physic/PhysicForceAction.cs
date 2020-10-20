using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Global.Physic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicForceAction : MonoBehaviour {
    public List < (int, UnityAction<ActionData>) > ActionList => actionList;
    // *-------Private-------Private-------Private------- 
    private List < (int, UnityAction<ActionData>) > actionList = new List < (int, UnityAction<ActionData>) > ();

    private ActionData actionData;



    void FixedUpdate () {
        actionData = new ActionData ();
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