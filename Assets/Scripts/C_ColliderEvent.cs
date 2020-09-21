﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class C_ColliderEvent : MonoBehaviour {
    public float delay = 0;
    public List<GameObject> objectFilter = new List<GameObject> ();
    public Events events = new Events ();

    private void Start () {

    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (enabled) Main (other.gameObject, EventType.OnTriggerEnter, other);
    }
    private void OnTriggerStay2D (Collider2D other) {
        if (enabled) Main (other.gameObject, EventType.OnTriggerStay, other);
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (enabled) Main (other.gameObject, EventType.OnTriggerExit, other);
    }
    private void OnCollisionEnter2D (Collision2D other) {
        if (enabled) Main (other.gameObject, EventType.OnCollisionEnter, collision : other);
    }
    private void OnCollisionStay2D (Collision2D other) {
        if (enabled) Main (other.gameObject, EventType.OnCollisionStay, collision : other);
    }
    private void OnCollisionExit2D (Collision2D other) {
        if (enabled) Main (other.gameObject, EventType.OnCollisionExit, collision : other);
    }


    //*Private Method
    private void Main (GameObject touchObj, EventType type, Collider2D collider = null, Collision2D collision = null) {
        if (enabled) {

            bool filterTestPass = true;

            filterTestPass = objectFilter.Contains (touchObj);



            if (filterTestPass) {
                CallEvent (delay, type, collider, collision);
            }


        }
    }

    private void CallEvent (float waitTime, EventType typ, Collider2D collider = null, Collision2D collision = null) {
        if (waitTime == 0) {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    events.onCollisionEnter.Invoke (collision);
                    break;
                case EventType.OnCollisionStay:
                    events.onCollisionStay.Invoke (collision);
                    break;
                case EventType.OnCollisionExit:
                    events.OnCollisionExit.Invoke (collision);
                    break;
                case EventType.OnTriggerEnter:
                    events.onTriggerEnter.Invoke (collider);
                    break;
                case EventType.OnTriggerStay:
                    events.onTriggerStay.Invoke (collider);
                    break;
                case EventType.OnTriggerExit:
                    events.OnTriggerExit.Invoke (collider);
                    break;
            }
        } else {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    Fn._.WaitToCall (waitTime, () => events.onCollisionEnter.Invoke (collision));
                    break;
                case EventType.OnCollisionStay:
                    Fn._.WaitToCall (waitTime, () => events.onCollisionStay.Invoke (collision));
                    break;
                case EventType.OnCollisionExit:
                    Fn._.WaitToCall (waitTime, () => events.OnCollisionExit.Invoke (collision));
                    break;
                case EventType.OnTriggerEnter:
                    Fn._.WaitToCall (waitTime, () => events.onTriggerEnter.Invoke (collider));
                    break;
                case EventType.OnTriggerStay:
                    Fn._.WaitToCall (waitTime, () => events.onTriggerStay.Invoke (collider));
                    break;
                case EventType.OnTriggerExit:
                    Fn._.WaitToCall (waitTime, () => events.OnTriggerExit.Invoke (collider));
                    break;
            }
        }

    }




    //*Public Method

    public static C_ColliderEvent AddCollierEvent (
        GameObject gameObject,
        GameObject[] targetObjects = null,
        UnityAction<Collider2D> onTriggerEnter = null,
        UnityAction<Collider2D> onTriggerStay = null,
        UnityAction<Collider2D> OnTriggerExit = null,
        UnityAction<Collision2D> OnCollisionEnter = null,
        UnityAction<Collision2D> OnCollisionStay = null,
        UnityAction<Collision2D> OnCollisionExit = null
    ) {


        C_ColliderEvent comp = gameObject.AddComponent<C_ColliderEvent> ();

        if (targetObjects != null) {
            if (targetObjects.Length > 0) {
                comp.objectFilter.AddRange (targetObjects);
            }
        }
        if (onTriggerEnter != null) {
            comp.events.onTriggerEnter.AddListener (onTriggerEnter);
        }
        if (onTriggerStay != null) {
            comp.events.onTriggerStay.AddListener (onTriggerStay);
        }
        if (OnTriggerExit != null) {
            comp.events.OnTriggerExit.AddListener (OnTriggerExit);
        }
        if (OnCollisionEnter != null) {
            comp.events.onCollisionEnter.AddListener (OnCollisionEnter);
        }
        if (OnCollisionStay != null) {
            comp.events.onCollisionStay.AddListener (OnCollisionStay);
        }
        if (OnCollisionExit != null) {
            comp.events.OnCollisionExit.AddListener (OnCollisionExit);
        }

        return comp;
    }


    //*Property
    [System.Serializable]
    public class Events {
        public UnityEvent<Collider2D> onTriggerEnter = new UnityEvent<Collider2D> ();
        public UnityEvent<Collider2D> onTriggerStay = new UnityEvent<Collider2D> ();
        public UnityEvent<Collider2D> OnTriggerExit = new UnityEvent<Collider2D> ();
        public UnityEvent<Collision2D> onCollisionEnter = new UnityEvent<Collision2D> ();
        public UnityEvent<Collision2D> onCollisionStay = new UnityEvent<Collision2D> ();
        public UnityEvent<Collision2D> OnCollisionExit = new UnityEvent<Collision2D> ();

    }

    [System.Serializable]
    public class GameObjectFilter {
        public bool objectFilter;
        public GameObject gameObject;
    }

    public enum EventType {
        OnTriggerEnter,
        OnTriggerStay,
        OnTriggerExit,
        OnCollisionEnter,
        OnCollisionStay,
        OnCollisionExit
    }

}


public static class Extension_C_ColliderEvent {
    public static C_ColliderEvent Ex_AddCollierEvent (this Collider2D collider,
            GameObject[] targetObjects = null,
            UnityAction<Collider2D> onTriggerEnter = null,
            UnityAction<Collider2D> onTriggerStay = null,
            UnityAction<Collider2D> OnTriggerExit = null,
            UnityAction<Collision2D> OnCollisionEnter = null,
            UnityAction<Collision2D> OnCollisionStay = null,
            UnityAction<Collision2D> OnCollisionExit = null
        ) =>

        C_ColliderEvent.AddCollierEvent (
            collider.gameObject,
            targetObjects,
            onTriggerEnter,
            onTriggerStay,
            OnTriggerExit,
            OnCollisionEnter,
            OnCollisionStay,
            OnCollisionExit
        );

}