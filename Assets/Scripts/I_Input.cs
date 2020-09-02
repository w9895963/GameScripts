using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Input : IC_Base {
    public Collider2D triggerBox;

    public InputType inputType = new InputType ();




    public override void OnEnable_ () {
        if (triggerBox) {
            if (inputType.click) {
                EventTrigger eventTrigger = triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerClick, (d) => {
                    Exit ();
                });
                data.tempInstance.AddIfEmpty (0, eventTrigger);
            }

            if (inputType.down) {
                data.tempInstance.Add (() =>
                    triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerDown, (d) => {
                        Exit ();
                    })
                );
            }
        }
    }
    public override void OnDisable_ () { }




    private void Exit () {
        behaviour.actionIndex = 0;
        enabled = false;
    }

    [System.Serializable]
    public class InputType {
        public bool click = true;
        public bool down = true;
    }


}