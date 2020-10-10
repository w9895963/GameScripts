using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_CursorEvent : MonoBehaviour {
    public Collider2D trigger;
    public M_Cursor.StateCs cursorState = default;
    public TempObject temp = new TempObject ();

    private void OnEnable () {
        if (trigger) {
            M_Cursor cursorCtrl = FindObjectOfType<M_Cursor> ();
            temp.AddEventTrigger = trigger.gameObject._Ex (this)
                .AddPointerEvent (EventTriggerType.PointerEnter, (d) => {
                    cursorCtrl.State = cursorState;
                });
            temp.AddEventTrigger = trigger.gameObject._Ex (this)
                .AddPointerEvent (EventTriggerType.PointerExit, (d) => {
                    cursorCtrl.State = M_Cursor.StateCs.normal;
                });
        }

    }
    private void OnDisable () {
        temp.DestroyAll ();
    }
}