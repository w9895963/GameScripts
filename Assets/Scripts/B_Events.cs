using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class B_Events : MonoBehaviour {
    [HideInInspector]
    public Events events;

    private void OnCollisionEnter2D (Collision2D other) {
        events.onCollisionEnter2D.Invoke (other);
    }
    private void OnCollisionExit2D (Collision2D other) {
        events.onCollisionExit2D.Invoke (other);
    }

    private void OnTriggerEnter2D (Collider2D other) {
        events.onTriggerEnter2D.Invoke (other);
    }
    private void OnTriggerExit2D (Collider2D other) {
        events.onTriggerExit2D.Invoke (other);
    }

    [System.Serializable]
    public class Events {
        public UnityEvent<Collision2D> onCollisionEnter2D;
        public UnityEvent<Collision2D> onCollisionExit2D;
        public UnityEvent<Collider2D> onTriggerEnter2D;
        public UnityEvent<Collider2D> onTriggerExit2D;

    }
}