using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDebugComp : MonoBehaviour
{
    public List<StateBundle.StateInst> allStates;
    public string states;

    void Update()
    {
        allStates = StateBundle.GlobalDate.stateDateDic[gameObject].AllEnableStates;
        states = "";
        allStates.ForEach((st) => states += st.GetType().ToString());
    }
}
