using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public GameObject follow;


    void Update () {


    }

    private void LateUpdate () {
        // Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        // rb.position = follow.transform.position;
        Vector3 p = follow.transform.position;
        p.z = transform.position.z;

        transform.position = p;
    }
}