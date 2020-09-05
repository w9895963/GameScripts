using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class I_GlobleState : IC_Base {

    public static List<string> currentState = new List<string> ();
    public string CurrentState;
    public int workingSlot = 0;
    public static List<I_GlobleState> allInstance = new List<I_GlobleState> ();
    public List<string> allStates = new List<string> ();
    public Action action = Action.none;
    public enum Action { none, setState, enableWithState }
    public string stateName;


    private void Awake () {
        if (currentState.Count <= workingSlot) {
            currentState.Add (workingSlot, default);
        }
        AddStateObject ();
        ShowAllName ();
    }
     void OnEnable () {
        if (action == Action.setState) {
            ChangeState (stateName);
        }
    }




    //* Private Method
    private void AddStateObject () {
        if (!allInstance.Contains (this)) {
            allInstance.Add (this);
        }
    }
    private void ShowAllName () {
        allInstance.RemoveAll ((x) => x == null);
        allInstance.ForEach ((x) => {
            x.allStates = allInstance.Select ((y) => y.stateName).ToList ();
            x.allStates.RemoveAll ((y) => y == "");

            x.CurrentState = currentState[workingSlot];
        });


    }

    public void ChangeState (string name) {
        allInstance.RemoveAll ((x) => x == null);
        string curr = currentState[workingSlot];
        if (curr != name & name != "") {
            curr = name;
            var lists = allInstance.FindAll ((x) => x.action == Action.enableWithState);
            Debug.Log (lists.Count);

            lists.FindAll ((x) => x.stateName == curr).ForEach ((x) => {
                x.enabled = true;
            });
            lists.FindAll ((x) => x.stateName != curr).ForEach ((x) => {
                x.enabled = false;
            });

        }

        CurrentState = curr;
    }
}