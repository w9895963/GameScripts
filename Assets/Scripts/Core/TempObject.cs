using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable] public class TempObject {
    private UnityEvent destroyCall = new UnityEvent ();
    [SerializeField] private List<Object> tempObjects = new List<Object> ();

    public void Add (EventTrigger eventTrigger, EventTrigger.Entry entry) {
        tempObjects.Add (eventTrigger);
        int index = tempObjects.Count - 1;
        destroyCall.AddListener (() => {
            EventTrigger comp = tempObjects[index] as EventTrigger;
            if (comp) {
                comp.triggers.Remove (entry);
                if (comp.triggers.Count == 0) {
                    comp.Destroy ();
                }
            }

        });

    }
    public void Add (Object obj) {
        tempObjects.Add (obj);
        int index = tempObjects.Count - 1;
        destroyCall.AddListener (() => {
            Object tempobj = tempObjects[index];
            if (tempobj) tempobj.Destroy ();
        });
    }

    public (EventTrigger, EventTrigger.Entry) AddEventTrigger {
        set {
            Add (value.Item1, value.Item2);
        }
    }
    public Object AddObject {
        set {
            Add (value);
        }
    }

    public void DestroyAll () {
        destroyCall.Invoke ();
        destroyCall.RemoveAllListeners ();
        tempObjects.Clear ();
    }
}