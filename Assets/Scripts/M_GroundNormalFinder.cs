using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_GroundNormalFinder : MonoBehaviour {
    [Header ("Import")]
    public Rigidbody2D mainRigidbody;
    public M_Gravity importGravity;
    [Tooltip ("Triger Box To Find Ground")]
    public B_Events trigerBox;
    public Vector2 gravity;
    [Header ("Output")]
    public Vector2 normal;
    public GameObject[] grounds;

    private void Awake () {
        // importGravity.GetComponent<M_Gravity> ().events.gravityUpdate.AddListener ((Vector2 v) => gravity = v);

        trigerBox.events.onTriggerEnter2D.AddListener ((Collider2D other) => {
            bool groundDetect = other.gameObject.tag == "Ground";
            if (groundDetect) {
                grounds = Fn.ArrayAddUniq (grounds, other.gameObject);
            }
            if (groundDetect) {
                normal = GetNormal () != default (Vector2) ? GetNormal () : normal;
                // output.setNormal.Invoke (normal);
            }
        });

        trigerBox.events.onTriggerExit2D.AddListener ((Collider2D other) => {
            bool isGround = other.gameObject.tag == "Ground";
            if (isGround) {
                grounds = Fn.ArrayRemove (grounds, other.gameObject);
            }

            if (grounds.Length > 0) {
                normal = GetNormal () != default (Vector2) ? GetNormal () : normal;
                // output.setNormal.Invoke (normal);


            } else {
                normal = default (Vector2);
                // output.setNormal.Invoke (normal);
                // output.onLeaveGround.Invoke ();

            }
        });
    }

    private Vector2 GetNormal () {
        gravity = importGravity.GetGravity ();

        RaycastHit2D[] hitsArray = new RaycastHit2D[32];
        ContactFilter2D filter = new ContactFilter2D ();
        filter.layerMask = LayerMask.NameToLayer ("ground");
        int count = mainRigidbody.GetComponent<Rigidbody2D> ().Cast (gravity, filter, hitsArray, 0.7f);
        List<RaycastHit2D> hits = new List<RaycastHit2D> (hitsArray).GetRange (0, count);


        hits = hits.FindAll ((RaycastHit2D hit) => Vector2.Angle (-gravity, hit.normal) < 45);
        hits.Sort ((RaycastHit2D hit1, RaycastHit2D hit2) => {
            Vector2 p = mainRigidbody.transform.position;
            float length1 = (hit1.point - p).magnitude;
            float length2 = (hit2.point - p).magnitude;
            return length1 > length2 ? 1 : -1;
        });
        if (hits.Count > 0) {
            return hits[0].normal;
        } else {
            return default (Vector2);
        }

    }

    public Vector2 GetGroundNormal () {
        return normal;
    }
    public bool HasGroundNormal () {
        return normal == default (Vector2) ? false : true;
    }


}