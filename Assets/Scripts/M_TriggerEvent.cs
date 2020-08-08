using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class M_TriggerEvent : MonoBehaviour {
    public bool useFilter = true;
    public float delay = 0;
    public bool runOnce = false;
    private bool used = false;
    public GameObjectFilter gameObjectFilter;
    public Events events;

    private void OnTriggerEnter2D (Collider2D other) {
        Main (other.gameObject, EventType.OnTriggerEnter);
    }
    private void OnCollisionEnter2D (Collision2D other) {
        Main (other.gameObject, EventType.OnCollisionEnter);
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
                if (runOnce) {
                    if (!used) {
                        CallEvent (delay, type);
                        used = true;
                    }
                } else {
                    CallEvent (delay, type);
                }
            }


        }
    }

    private void CallEvent (float waitTime, EventType typ) {
        if (waitTime == 0) {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    events.onCollisionEnter.Invoke ();
                    break;
                case EventType.OnTriggerEnter:
                    events.onTriggerEnter.Invoke ();
                    break;
            }
        } else {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    Fn.WaitToCall (waitTime, () => events.onCollisionEnter.Invoke ());
                    break;
                case EventType.OnTriggerEnter:
                    Fn.WaitToCall (waitTime, () => events.onTriggerEnter.Invoke ());
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
        public UnityEvent onCollisionEnter;

    }

    public enum EventType { OnTriggerEnter, OnCollisionEnter }

    private void Start () {

    }



    //*Extend Method
    public void AnimationPlay () {
        GetComponent<Animation> ()?.Play ();
    }
}