using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicEventHandler : MonoBehaviour {
    public UnityEvent<Collision2D> onCollisionEnter2D = new UnityEvent<Collision2D> ();
    public UnityEvent<Collision2D> onCollisionStay2D = new UnityEvent<Collision2D> ();
    public UnityEvent<Collision2D> onCollisionExit2D = new UnityEvent<Collision2D> ();
    private void OnCollisionEnter2D (Collision2D other) {
        onCollisionEnter2D.Invoke ((other));
    }
    private void OnCollisionStay2D (Collision2D other) {
        onCollisionStay2D.Invoke ((other));
    }
    private void OnCollisionExit2D (Collision2D other) {
        onCollisionExit2D.Invoke ((other));
    }
}