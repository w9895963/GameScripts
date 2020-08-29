using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class C_StateMachine {
    private State[] states = new State[0];
    private State currentState;


    public C_StateMachine (params string[] states) {
        this.states = this.states.Add (new State (states[0]));
        currentState = this.states[0];
        for (int i = 1; i < states.Length; i++) {
            this.states = this.states.Add (new State (states[i]));
        }
    }




    public string CurrentState { get => currentState.name; }
    public string PreStateName { get => currentState.prevState.name; }
    public string NextStateName { get => currentState.nextState.name; }
    public bool IsCurrent (System.Enum e) {
        return currentState.name == e.ToString ();
    }

    public void ChangeState (string name) {
        State state = states.Find ((i) => i.name == name);
        if (state.name != currentState.name) {
            currentState.nextState = state;
            state.prevState = currentState;
            currentState.exit.Invoke ();
            currentState = state;
            currentState.enter.Invoke ();
        }
    }
    public void ChangeState (System.Enum e) {
        ChangeState (e.ToString ());
    }
    public void SetState (string name) {
        currentState = states.Find ((i) => i.name == name);
    }
    public void AddListener (System.Enum e, UnityAction enter = null, UnityAction exit = null) {
        if (enter != null) states.Find ((s) => s.name == e.ToString ()).enter.AddListener (enter);
        if (exit != null) states.Find ((s) => s.name == e.ToString ()).exit.AddListener (exit);
    }
    public void AddExitListener (UnityAction exit = null, params System.Enum[] es) {
        List<System.Enum> esl = new List<System.Enum> (es);
        List<string> names = new List<string> ();
        esl.ForEach (x => {
            names.Add (x.ToString ());
        });
        Debug.Log (names);
        foreach (string name in names) {
            if (exit != null) states.Find ((s) => s.name == name).exit.AddListener (exit);

        }
    }
    public void AddEnterListener (UnityAction enter = null, params System.Enum[] es) {
        List<System.Enum> esl = new List<System.Enum> (es);
        List<string> names = new List<string> ();
        esl.ForEach (x => {
            names.Add (x.ToString ());
        });
        foreach (string name in names) {
            if (enter != null) states.Find ((s) => s.name == name).enter.AddListener (enter);
        }
    }
    public void AddListenerToAll (UnityAction enter = null, UnityAction exit = null) {
        foreach (State st in states) {
            if (enter != null) st.enter.AddListener (enter);
            if (exit != null) st.exit.AddListener (exit);
        }

    }
    public void InvokeEnterEvent (string name) {
        states.Find ((s) => s.name == name).enter.Invoke ();
    }
    public void InvokeExitEvent (string name) {
        states.Find ((s) => s.name == name).exit.Invoke ();
    }


    public class State {
        public string name;
        public State prevState;
        public State nextState;
        public UnityEvent enter = new UnityEvent ();
        public UnityEvent exit = new UnityEvent ();

        public State (string name) {
            this.name = name;
        }
    }

}