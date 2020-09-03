using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IG_SImple : IG_GroupCore {
    [ReadOnly] public string currentState;
    public string activeState;
    public IC_Base setOn;
    public GameObject setOff;




    public new void OnValidate () {
        if (!string.IsNullOrWhiteSpace (activeState)) property.allowStates.Add (0, activeState);
        if (setOn) property.setOn.Add (0, setOn);
        if (setOff) property.setOff = setOff.GetComponents<IC_Base> ().ToList ();
        property.group = GetComponents<IG_GroupCore> ().ToList ();




        base.OnValidate ();


    }



    public override void PropertyChanged (string current) {
        currentState = current;
    }
}