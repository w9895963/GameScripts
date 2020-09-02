using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_GlobleState : IC_Base {

    public static string currentState;
    public string CurrentState;
    public static List<I_GlobleState> allInstance = new List<I_GlobleState> ();
    public List<string> allStates = new List<string> ();
    public string enableWithStateName;
    public bool enableWithState = false;
    public string setStatename;
    public bool setCurrentState = false;
    public override void OnEnable_ () {
        if (setCurrentState) {
            ChangeState (setStatename);
        }

    }

    public override void OnDisable_ () {

    }

    private void Reset () {
        AddState ();
        ShowAllName ();
    }



    private void OnDestroy () {
        allInstance.Remove (this);
    }
    private void OnValidate () {
        if (enabled) {
            if (setCurrentState) {
                ChangeState (setStatename);
            }
        }
        AddState ();
        ShowAllName ();
    }



    //* Private Method
    private void AddState () {
        if (!allInstance.Contains (this)) {
            allInstance.Add (this);
        }
    }
    private void ShowAllName () {
        allInstance.RemoveAll ((x) => x == null);
        allStates.Clear ();
        allInstance.ForEach ((x) => {
            if (!allStates.Contains (x.enableWithStateName) & x.enableWithStateName != "") {
                allStates.Add (x.enableWithStateName);
            }
            if (!allStates.Contains (x.setStatename) & x.setStatename != "") {
                allStates.Add (x.setStatename);
            }
        });

        CurrentState = currentState;
    }

    public void ChangeState (string name) {
        allInstance.RemoveAll ((x) => x == null);
        if (currentState != name & name != "") {
            currentState = name;
            allInstance.ForEach ((s) => {
                if (s.enableWithState) {
                    if (s.enableWithStateName == currentState) {
                        s.enabled = true;
                    } else {
                        s.enabled = false;
                    }
                }
            });
        }

        CurrentState = currentState;
    }
}