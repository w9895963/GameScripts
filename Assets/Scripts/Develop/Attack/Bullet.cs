using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> particleCollisionEvent = new List<ParticleCollisionEvent>();
        GetComponent<ParticleSystem>().GetCollisionEvents(other, particleCollisionEvent);
        Vector2 v = particleCollisionEvent[0].velocity.ToVector2().Project(Vector2.right);
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            return;
        }
        rb.AddForce(v);
    }
}
