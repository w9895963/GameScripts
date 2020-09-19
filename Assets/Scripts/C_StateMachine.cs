using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class C_StateMachine<T> where T : System.Enum {
    private List<StateObj> states = new List<StateObj> ();
    private StateObj currentState;


    public C_StateMachine () {
        foreach (T t in System.Enum.GetValues (typeof (T))) {
            states.Add (new StateObj (t));
        };
        currentState = states[0];

    }




    public T CurrentState { get => currentState.state; }
    public T PreState { get => currentState.prevState.state; }
    public T NextState { get => currentState.nextState.state; }
    public bool IsCurrent (T e) {
        return currentState.name == e.ToString ();
    }

    public void ChangeState (T state) {
        StateObj stateObj = states.Find ((i) => i.name == state.ToString ());
        if (stateObj.name != currentState.name) {
            currentState.nextState = stateObj;
            stateObj.prevState = currentState;
            currentState.exit.Invoke ();
            currentState = stateObj;
            currentState.enter.Invoke ();
        }
    }
    public void SetState (T e) {
        currentState = states.Find ((i) => i.name == e.ToString ());
    }
    public void AddListener (T e, UnityAction enter = null, UnityAction exit = null) {
        if (enter != null) states.Find ((s) => s.name == e.ToString ()).enter.AddListener (enter);
        if (exit != null) states.Find ((s) => s.name == e.ToString ()).exit.AddListener (exit);
    }
    public void AddExitListener (UnityAction exit = null, params T[] es) {
        foreach (T t in es) {
            if (exit != null) states.Find ((s) => s.name == t.ToString ()).exit.AddListener (exit);
        }
    }
    public void AddEnterListener (UnityAction enter = null, params T[] es) {
        foreach (T t in es) {
            if (enter != null) states.Find ((s) => s.name == t.ToString ()).enter.AddListener (enter);
        }
    }
    public void AddListenerToAll (UnityAction enter = null, UnityAction exit = null) {
        foreach (StateObj st in states) {
            if (enter != null) st.enter.AddListener (enter);
            if (exit != null) st.exit.AddListener (exit);
        }

    }
    public void InvokeEnterEvent (T t) {
        states.Find ((s) => s.state.Equals (t)).enter.Invoke ();
    }
    public void InvokeExitEvent (T state) {
        states.Find ((s) => s.state.Equals (state)).exit.Invoke ();
    }


    public class StateObj {
        public string name;
        public T state;
        public StateObj prevState;
        public StateObj nextState;
        public UnityEvent enter = new UnityEvent ();
        public UnityEvent exit = new UnityEvent ();

        public StateObj (T t) {
            this.state = t;
            this.name = t.ToString ();
        }
    }

    public class Between {
        public StateObj prevState;
        public StateObj nextState;
        public UnityAction action;

    }

}