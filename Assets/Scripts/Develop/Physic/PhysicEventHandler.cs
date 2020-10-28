using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicEventHandler : MonoBehaviour {
    public int eventCount = 0;
    public UnityEvent<Collision2D> onCollisionEnter2D = new UnityEvent<Collision2D> ();
    public UnityEvent<Collision2D> onCollisionStay2D = new UnityEvent<Collision2D> ();
    public UnityEvent<Collision2D> onCollisionExit2D = new UnityEvent<Collision2D> ();
    public UnityEvent<Collider2D> onTriggerEnter2D = new UnityEvent<Collider2D> ();
    public UnityEvent<Collider2D> onTriggerStay2D = new UnityEvent<Collider2D> ();
    public UnityEvent<Collider2D> onTriggerExit2D = new UnityEvent<Collider2D> ();
    private void OnCollisionEnter2D (Collision2D other) {
        onCollisionEnter2D.Invoke ((other));
    }
    private void OnCollisionStay2D (Collision2D other) {
        onCollisionStay2D.Invoke ((other));
    }
    private void OnCollisionExit2D (Collision2D other) {
        onCollisionExit2D.Invoke ((other));
    }
    private void OnTriggerEnter2D (Collider2D other) {
        onTriggerEnter2D.Invoke (other);
    }
    private void OnTriggerStay2D (Collider2D other) {
        onTriggerStay2D.Invoke (other);
    }
    private void OnTriggerExit2D (Collider2D other) {
        onTriggerExit2D.Invoke (other);
    }
}