using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_CursorEvent : MonoBehaviour {
    public Collider2D trigger;
    public string cursorState;
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
                    cursorCtrl.State = null;
                });
        }

    }
    private void OnDisable () {
        temp.DestroyAll ();
    }
}