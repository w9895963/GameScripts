using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDebugComp : MonoBehaviour
{
    public List<State.State> allStates;
    public string states;

    void Update()
    {
        allStates = State.GlobalDate.stateDateDic[gameObject].AllEnableStates;
        states = "";
        allStates.ForEach((st) => states += st.GetType().ToString());
    }
}
