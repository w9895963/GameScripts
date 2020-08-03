using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour {


    public M_Gravity importGravity;
    public int count;
    public float time;


    private void Awake () {
        EventTrigger.Entry trigger = new EventTrigger.Entry ();
        trigger.eventID = EventTriggerType.PointerUp;
        trigger.callback.AddListener (data => {
            if (Time.time - time > 0.2f & count > 0) {
                Vector2 dir = Fn.RotateClock (importGravity.GetGravity (), count * 90);
                importGravity.SetGravityDirection (dir);
                count = 0;
            }
        });
        GetComponent<EventTrigger> ().triggers.Add (trigger);

    }
   


    public void click () {
        count++;
        time = Time.time;
    }
}