using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_GameStateEvent : MonoBehaviour {
    public StateEvent[] stateEvents;



    void Start () {

    }

    [System.Serializable]
    public class StateEvent {
        public string name;
        public UnityEvent OnStateEnter;
        public UnityEvent OnStateExist;
    }

}