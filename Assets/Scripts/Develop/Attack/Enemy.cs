using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int count = 0;

    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent> ();

    private void OnParticleCollision (GameObject other) {
        ParticleSystem particle = other.GetComponent<ParticleSystem> ();
        int count = particle.GetCollisionEvents (gameObject, collisionEvents);
        Debug.Log (count);
    }
}