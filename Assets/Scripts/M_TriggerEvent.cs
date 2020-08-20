using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class M_TriggerEvent : MonoBehaviour {
    public bool useFilter = true;
    public float delay = 0;
    public GameObjectFilter gameObjectFilter;
    public Events events;

    private void OnTriggerEnter2D (Collider2D other) {
        Main (other.gameObject, EventType.OnTriggerEnter);
    }
    private void OnCollisionEnter2D (Collision2D other) {
        Main (other.gameObject, EventType.OnCollisionEnter);
    }
    private void OnTriggerExit2D (Collider2D other) {
        Main (other.gameObject, EventType.OnTriggerExit);
    }
    private void OnCollisionExit2D (Collision2D other) {
        Main (other.gameObject, EventType.OnCollisionExit);
    }

    private void Main (GameObject obj, EventType type) {
        if (enabled) {

            bool filterTest = false;

            if (gameObjectFilter.objectFilter) {
                if (obj == gameObjectFilter.gameObject) {
                    filterTest = true;
                }
            }

            if (!useFilter) {
                filterTest = true;
            }



            if (filterTest) {
                CallEvent (delay, type);
            }


        }
    }

    private void CallEvent (float waitTime, EventType typ) {
        if (waitTime == 0) {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    events.onCollisionEnter.Invoke ();
                    break;
                case EventType.OnCollisionExit:
                    events.OnCollisionExit.Invoke ();
                    break;
                case EventType.OnTriggerEnter:
                    events.onTriggerEnter.Invoke ();
                    break;
                case EventType.OnTriggerExit:
                    events.OnTriggerExit.Invoke ();
                    break;
            }
        } else {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    Fn.WaitToCall (waitTime, () => events.onCollisionEnter.Invoke ());
                    break;
                case EventType.OnCollisionExit:
                    Fn.WaitToCall (waitTime, () => events.OnCollisionExit.Invoke ());
                    break;
                case EventType.OnTriggerEnter:
                    Fn.WaitToCall (waitTime, () => events.onTriggerEnter.Invoke ());
                    break;
                case EventType.OnTriggerExit:
                    Fn.WaitToCall (waitTime, () => events.OnTriggerExit.Invoke ());
                    break;
            }
        }

    }

    [System.Serializable]
    public class GameObjectFilter {
        public bool objectFilter;
        public GameObject gameObject;
    }

    [System.Serializable]
    public class Events {
        public UnityEvent onTriggerEnter;
        public UnityEvent OnTriggerExit;
        public UnityEvent onCollisionEnter;
        public UnityEvent OnCollisionExit;

    }

    public enum EventType { OnTriggerEnter, OnTriggerExit, OnCollisionEnter, OnCollisionExit }

    private void Start () {

    }



}