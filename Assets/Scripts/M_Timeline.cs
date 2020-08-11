using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_Timeline : MonoBehaviour {
    public Animator animator;
    private Vector2 lastVector;
    public float z = 0;

    void Awake () {
        Fn.AddEventToTrigger (gameObject, EventTriggerType.Drag, dt => {
            PointerEventData data = (PointerEventData) dt;
            Vector2 pressPosition = data.pressPosition;
            Vector2 position = data.position;
            Vector2 vector = position - pressPosition;

            float angle = Vector2.SignedAngle (lastVector, vector);
            // float angle2 = Vector2.SignedAngle (Vector2.up, vector);

            transform.Rotate (Vector3.forward, angle);
            z += angle;
            z = z % 360;
            animator.SetFloat ("Angle", z);
            // var r = transform.rotation;
            // transform.rotation = Quaternion.Euler (r.x, r.y, z);
            // transform.rota
            // transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
            // timeline.time = angle / 360 + 1;
            // timeline.Evaluate ();

            lastVector = vector;
        });
        Fn.AddEventToTrigger (gameObject, EventTriggerType.EndDrag, dt => {
            lastVector = default;
        });

    }



    void Update () {

    }

}