using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class C_ColliderEvent : MonoBehaviour {
    [SerializeField] private float delay = 0;
    [SerializeField] private GameObjectFilter gameObjectFilter = new GameObjectFilter ();
    [SerializeField] private Events events = new Events ();



    private void OnTriggerEnter2D (Collider2D other) {
        Main (other.gameObject, EventType.OnTriggerEnter, other);
    }
    private void OnCollisionEnter2D (Collision2D other) {
        Main (other.gameObject, EventType.OnCollisionEnter);
    }
    private void OnTriggerExit2D (Collider2D other) {
        Main (other.gameObject, EventType.OnTriggerExit, other);
    }
    private void OnCollisionExit2D (Collision2D other) {
        Main (other.gameObject, EventType.OnCollisionExit);
    }


    //*Private Method
    private void Main (GameObject obj, EventType type, Collider2D collider = null) {
        if (enabled) {

            bool filterTest = true;

            if (gameObjectFilter.objectFilter) {
                if (obj != gameObjectFilter.gameObject) {
                    filterTest = false;
                }
            }




            if (filterTest) {
                CallEvent (delay, type, collider);
            }


        }
    }

    private void CallEvent (float waitTime, EventType typ, Collider2D collider = null) {
        if (waitTime == 0) {
            switch (typ) {
                case EventType.OnCollisionEnter:
                    events.onCollisionEnter.Invoke ();
                    break;
                case EventType.OnCollisionExit:
                    events.OnCollisionExit.Invoke ();
                    break;
                case EventType.OnTriggerEnter:
                    events.onTriggerEnter.Invoke (collider);
                    break;
                case EventType.OnTriggerExit:
                    events.OnTriggerExit.Invoke (collider);
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
                    Fn.WaitToCall (waitTime, () => events.onTriggerEnter.Invoke (collider));
                    break;
                case EventType.OnTriggerExit:
                    Fn.WaitToCall (waitTime, () => events.OnTriggerExit.Invoke (collider));
                    break;
            }
        }

    }




    //*Public Method
    public void SetObject (GameObject obj) {
        gameObjectFilter.gameObject = obj;
        gameObjectFilter.objectFilter = true;
    }
    public void AddListener (EventType type, UnityAction<Collider2D> action) {
        switch (type) {
            case EventType.OnTriggerEnter:
                events.onTriggerEnter.AddListener (action);
                break;
            case EventType.OnTriggerExit:
                events.OnTriggerExit.AddListener (action);
                break;
        }
    }
    public void AddListener (EventType type, UnityAction action) {
        switch (type) {
            case EventType.OnCollisionEnter:
                events.onCollisionEnter.AddListener (action);
                break;
            case EventType.OnCollisionExit:
                events.OnCollisionExit.AddListener (action);
                break;
        }
    }



    public static C_ColliderEvent AddTriggerEvent (
        GameObject gameObject,
        GameObject targetGameobject = null,
        UnityAction<Collider2D> enterAction = null,
        UnityAction<Collider2D> exitAction = null) {


        C_ColliderEvent comp = gameObject.AddComponent<C_ColliderEvent> ();
        if (targetGameobject != null) {
            comp.gameObjectFilter.objectFilter = true;
            comp.gameObjectFilter.gameObject = targetGameobject;
        }
        if (enterAction != null) {
            comp.events.onTriggerEnter.AddListener (enterAction);
        }
        if (exitAction != null) {
            comp.events.OnTriggerExit.AddListener (exitAction);
        }

        return comp;
    }


    //*Property
    [System.Serializable]
    public class Events {
        public UnityEvent<Collider2D> onTriggerEnter = new UnityEvent<Collider2D> ();
        public UnityEvent<Collider2D> OnTriggerExit = new UnityEvent<Collider2D> ();
        public UnityEvent onCollisionEnter = new UnityEvent ();
        public UnityEvent OnCollisionExit = new UnityEvent ();

    }

    [System.Serializable]
    public class GameObjectFilter {
        public bool objectFilter;
        public GameObject gameObject;
    }

    public enum EventType {
        OnTriggerEnter,
        OnTriggerExit,
        OnCollisionEnter,
        OnCollisionExit
    }

}


public static class _Extension_C_ColliderEvent {
    public static C_ColliderEvent AddTriggerEvent (this Fn fn,
            GameObject gameObject,
            GameObject targetGameobject = null,
            UnityAction<Collider2D> enterAction = null,
            UnityAction<Collider2D> exitAction = null) =>

        C_ColliderEvent.AddTriggerEvent (gameObject,
            targetGameobject,
            enterAction,
            exitAction);

    public static C_ColliderEvent Ex_AddTriggerEvent (this Collider2D collider,
            GameObject targetGameobject = null,
            UnityAction<Collider2D> enter = null,
            UnityAction<Collider2D> exit = null) =>

        C_ColliderEvent.AddTriggerEvent (collider.gameObject,
            targetGameobject,
            enter,
            exit);
}