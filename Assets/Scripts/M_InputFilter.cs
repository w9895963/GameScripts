using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_InputFilter : MonoBehaviour {
    [Header ("Import")]
    public GameObject innerZone;
    public M_Gravity gravityImport;
    public Vector2 gravity;

    [Header ("Output")]
    public Vector2 clickPosition;
    public Vector2 targetOnGround;

    [Header ("Output Event")]
    public Events events;


    private void Awake () {

        Fn.AddEventToTrigger (innerZone, EventTriggerType.PointerClick, (data) => {

            gravity = gravityImport.GetGravity ();
            PointerEventData dat = (PointerEventData) data;
            clickPosition = Camera.main.ScreenToWorldPoint (dat.position);


            RaycastHit2D hitG = Physics2D.Raycast (clickPosition, gravity, 10, LayerMask.GetMask ("Ground"));
            targetOnGround = hitG?hitG.point : default;


            events.click.Invoke ();
        });
    }

    private void Start () { }


    public Vector2 GetGroundPosition () => targetOnGround;

    public Vector2 GetClickPosition () => clickPosition;

    [System.Serializable]
    public class Events {

        public UnityEvent click;

    }

}