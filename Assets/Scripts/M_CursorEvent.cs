using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_CursorEvent : MonoBehaviour {
    public Collider2D trigger;
    public M_Cursor.StateCs cursorState = default;
    public List<Object> tempEvents = new List<Object> ();
    private void Awake () {

    }
    private void OnEnable () {
        if (trigger) {
            M_Cursor cursorCtrl = FindObjectOfType<M_Cursor> ();
            EventTrigger eve;
            eve = trigger.Ex_AddInputToTrigger (EventTriggerType.PointerEnter, (d) => {
                cursorCtrl.State = cursorState;
            });
            tempEvents.Add (eve);
            eve = trigger.Ex_AddInputToTrigger (EventTriggerType.PointerExit, (d) => {
                cursorCtrl.State = M_Cursor.StateCs.normal;
            });
            tempEvents.Add (eve);
        }

    }
    private void OnDisable () {
        tempEvents.Destroy ();
    }
}