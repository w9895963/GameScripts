using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using static Fn;

public class InputManager : MonoBehaviour {
    public GameObject game_Object;
    public EventTrigger clickZone;
    public Collider2D collide;

    public bool setMovingPoint;
    public Vector2 movingPoint;

    [Header ("Event Out")]
    public UnityEvent<Vector2> onClick;


    private void Awake () {
        // clickZone.OnPointerDown(setMovingPoint);
        EventTrigger.Entry en = new EventTrigger.Entry ();
        en.eventID = EventTriggerType.PointerDown;
        en.callback.AddListener ((data => {

            //onClick.Invoke();
        }));
        clickZone.triggers.Add (en);


    }


}