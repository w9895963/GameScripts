using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_GroundFinder : MonoBehaviour {
    [Header ("Import")]
    public Rigidbody2D mainRigidbody;
    public M_Gravity importGravity;
    [Tooltip ("Triger Box To Find Ground")]
    public B_Events trigerBox;
    public Vector2 gravity;
    [Header ("Output")]
    public Vector2 normal;
    public GameObject groundOnFeet;
    public GameObject[] grounds;

    private void Awake () {
        importGravity.GetComponent<M_Gravity> ().events.gravityChanged.AddListener (() => {
            normal = FindNormal ();
            FindGround ();
        });

        trigerBox.events.onTriggerEnter2D.AddListener ((Collider2D other) => {
            // bool groundDetect = other.gameObject.tag == "Ground";
            bool groundDetect = other.gameObject.layer == LayerMask.NameToLayer("Ground");
            if (groundDetect) {
                grounds = Fn.ArrayAddUniq (grounds, other.gameObject);
            }
            if (groundDetect) {
                normal = FindNormal () != default (Vector2) ? FindNormal () : normal;

            }

            FindGround ();
        });

        trigerBox.events.onTriggerExit2D.AddListener ((Collider2D other) => {
            // bool isGround = other.gameObject.tag == "Ground";
            bool isGround = other.gameObject.layer == LayerMask.NameToLayer("Ground");
            if (isGround) {
                grounds = Fn.ArrayRemove (grounds, other.gameObject);
            }

            if (grounds.Length > 0) {
                normal = FindNormal () != default (Vector2) ? FindNormal () : normal;


            } else {
                normal = default (Vector2);

            }


            FindGround ();
        });


    }

    private void FindGround () {
        if (grounds.Length > 0) {
            RaycastHit2D hit = Physics2D.Raycast (mainRigidbody.position, gravity, 1.5f, LayerMask.GetMask ("Ground"));

            if (hit) {
                // Debug.Log (hit.collider.name);

                groundOnFeet = hit.collider.gameObject;

            } else {
                if (normal != default)
                    groundOnFeet = grounds[0];
                else
                    groundOnFeet = default;
            }
        } else {
            groundOnFeet = default;
        }

    }

    private Vector2 FindNormal () {
        gravity = importGravity.GetGravity ();

        RaycastHit2D[] hitsArray = new RaycastHit2D[32];
        ContactFilter2D filter = new ContactFilter2D ();
        filter.layerMask = LayerMask.GetMask ("Ground");

        int count = mainRigidbody.GetComponent<Rigidbody2D> ().Cast (gravity, filter, hitsArray, 0.5f);
        List<RaycastHit2D> hits = new List<RaycastHit2D> (hitsArray).GetRange (0, count);


        hits = hits.FindAll ((RaycastHit2D hit) => Vector2.Angle (-gravity, hit.normal) < 45);
        hits.Sort ((RaycastHit2D hit1, RaycastHit2D hit2) => {
            Vector2 p = mainRigidbody.transform.position;
            float length1 = (hit1.point - p).magnitude;
            float length2 = (hit2.point - p).magnitude;
            return length1 > length2 ? 1 : -1;
        });
        if (hits.Count > 0) {
            // Fn.DrawVector (hits[0].point, hits[0].normal);
            return hits[0].normal;
        } else {
            return default (Vector2);
        }

    }


    //*Public Method
    public Vector2 GetGroundNormal () {
        return normal;
    }
    public GameObject GetGround () {
        return groundOnFeet;
    }
    public bool IsOnGround () {

        return grounds.Length > 0 ? true : false;
    }


}