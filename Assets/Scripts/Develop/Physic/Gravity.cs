using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector2 gravityForce = new Vector2(0, -80);
    private void FixedUpdate()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(gravityForce);
    }
}


