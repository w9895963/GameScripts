using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class I_Base : MonoBehaviour {
    public bool autoQuit = true;
    public Events events = new Events ();
    public List<Object> elist = new List<Object> (0);

    private void OnEnable () {
        events.exit.Invoke ();
    }

    private void OnDisable () {
        if (autoQuit) {

        }
        events.enter.Invoke ();
    }

    public void Enable () => enabled = true;
    public void Disable () => enabled = false;

    [System.Serializable]
    public class Events {
        public UnityEvent enter = new UnityEvent ();
        public UnityEvent exit = new UnityEvent ();
    };
}