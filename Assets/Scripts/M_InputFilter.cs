using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_InputFilter : MonoBehaviour {
    [Header ("Import")]
    public EventTrigger clickZone;
    public M_Gravity gravityImport;
    public Vector2 gravity;

    [Header ("Output")]
    public Vector2 targetInner;
    public Vector2 targetOnGround;

    [Header ("Output Event")]
    public Events events;


    private void Awake () {
        EventTrigger.Entry en = new EventTrigger.Entry ();
        en.eventID = EventTriggerType.PointerDown;
        en.callback.AddListener ((a => {
            if (enabled) {
                gravity = gravityImport.GetGravity ();
                Rigidbody2D rb = GetComponent<Rigidbody2D> ();

                Vector2 clickPosition = Camera.main.ScreenToWorldPoint (Pointer.current.position.ReadValue ());
                //Vector2 clickPosition = Camera.main.ScreenToWorldPoint (Mouse.current.position.ReadValue ());
                Vector2 playerPosition = transform.position;

                bool isInner;
                RaycastHit2D[] hits = Physics2D.RaycastAll (clickPosition, Vector2.zero, 0f, LayerMask.GetMask ("Zone"));
                List<RaycastHit2D> hits_ = new List<RaycastHit2D> (hits);

                isInner = hits_.Exists (hit => hit.collider.name == "InnerZone");

                if (isInner) {
                    targetInner = clickPosition;

                    RaycastHit2D hitG = Physics2D.Raycast (targetInner, gravity, 8, LayerMask.GetMask ("Ground"));
                    targetOnGround = hitG?hitG.point : default;


                    events.click.Invoke ();


                } else {
                    targetInner = default;
                    targetOnGround = default;
                }

            }

        }));

        clickZone.triggers.Add (en);
    }

    private void Start () {

    }


    public Vector2 GetGroundPosition () {
        return targetOnGround;
    }
    public Vector2 GetTargetInner () {
        return targetInner;
    }

    [System.Serializable]
    public class Events {

        public UnityEvent click;

    }

}