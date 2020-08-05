using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_TriggerEvent : MonoBehaviour {
    public bool oneTimeEvent;
    public bool useFilter;
    private bool used = false;
    public ObjectTouch gameObjectFilter;
    public UnityEvent onTouch;

    private void OnTriggerEnter2D (Collider2D other) {
        if (enabled) {

            bool filterTest = false;


            if (gameObjectFilter.enable) {
                if (other.gameObject == gameObjectFilter.gameObject) {
                    filterTest = true;
                }
            }


            if (!useFilter) {
                filterTest = true;
            }


            if (filterTest) {
                if (oneTimeEvent) {
                    if (!used) {
                        onTouch.Invoke ();
                        used = true;
                    }
                } else {
                    onTouch.Invoke ();
                }
            }


        }
    }

    [System.Serializable]
    public class ObjectTouch {
        public bool enable;
        public GameObject gameObject;
    }

    private void Start () {

    }
}