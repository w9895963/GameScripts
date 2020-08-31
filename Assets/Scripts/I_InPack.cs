using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_InPack : MonoBehaviour {
    public GameObject targetObject;
    public bool autoQuit = true;
    public Events events = new Events ();
    public List<Object> elist = new List<Object> (0);


    private void OnEnable () {
        Enter ();


        gameObject.transform.position += new Vector3 (0, 0, -100);
        gameObject.Ex_Hide ();
        Gb.Backpack.PutinStorage (targetObject);


    }
    private void OnDisable () {


        Exit ();

    }

    private void Enter () {
        events.inEvent.Invoke ();
    }


    private void Exit () {
        if (autoQuit) {
            var c = GetComponent<I_Placing> ();
           
        }
        events.outEvent.Invoke ();
    }

    [System.Serializable]
    public class Events {
        public UnityEvent outEvent = new UnityEvent ();
        public UnityEvent inEvent = new UnityEvent ();
    };
}