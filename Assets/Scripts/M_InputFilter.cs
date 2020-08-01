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
        en.callback.AddListener (((a) => {
            gravity = gravityImport.GetGravity ();

            Vector2 clickPosition = Camera.main.ScreenToWorldPoint (Pointer.current.position.ReadValue ());
            Vector2 playerPosition = transform.position;

            bool isInner;
            RaycastHit2D[] hits = Physics2D.RaycastAll (clickPosition, Vector2.zero, 0f, LayerMask.GetMask ("Zone"));
            List<RaycastHit2D> hits_ = new List<RaycastHit2D> (hits);
            isInner = hits_.Exists (hit => hit.rigidbody.name == "InnerZone");
            Debug.Log(123);

            if (isInner) {
                targetInner = clickPosition;
                Fn.DrawCross (targetInner);
            }

            events.click.Invoke ();
           // D_DebugEventAction.CreateSign (targetInner);
        }));
        clickZone.triggers.Add (en);
    }

    [System.Serializable]
    public class Events {

        public UnityEvent click;
    }

}