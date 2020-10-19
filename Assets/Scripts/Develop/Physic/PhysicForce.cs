using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PhysicForce : MonoBehaviour {
    //* Public Method
    public void AddPhysicAction (int index, UnityAction<ActionData> action) {
        actionList.Add ((index, action));
        actionList.Sort ((x1, x2) => {
            if (x1.index < 0 & x2.index >= 0) {
                return 1;
            } else {
                return x1.index.CompareTo (x1.index);
            }
        });
    }
    public void RemovePhysicAction (UnityAction<ActionData> action) {
        actionList.RemoveAll ((x)=>x.action==action);
    }
    //* Class Definition
    public class ActionData {
        public Vector2 addForce;
    }

    // *-------Private-------Private-------Private------- 
    private List < (int index, UnityAction<ActionData> action) > actionList = new List < (int index, UnityAction<ActionData> action) > ();
    private ActionData acfionData = new ActionData ();


    void FixedUpdate () {
        actionList.ForEach ((x) => {
            x.action (acfionData);
            GetComponent<Rigidbody2D> ().AddForce (acfionData.addForce);
        });
    }



}