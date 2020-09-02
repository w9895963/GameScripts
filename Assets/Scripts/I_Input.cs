using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Input : IC_Base {
    [System.Serializable] public class Setting {
        public bool useTriggerBox = true;
        public Collider2D triggerBox;
        public InputType inputType = new InputType ();
    }
    public Setting setting = new Setting ();




    public override void OnEnable_ () {
        data.actionIndex = -1;
        Collider2D triggerBox = setting.triggerBox;
        if (setting.useTriggerBox) {
            if (triggerBox) {
                if (setting.inputType.click) {
                    EventTrigger eventTrigger = triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerClick, (d) => {
                        Exit ();
                    });
                    data.tempInstance.AddIfEmpty (0, eventTrigger);
                }

                if (setting.inputType.down) {
                    data.tempInstance.Add (() =>
                        triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerDown, (d) => {
                            Exit ();
                        })
                    );
                }
            }
        } else {
            if (setting.inputType.click) {
                data.tempInstance.Add (Fn._.AddPointerEvent (PointerEventType.onClick, (d) => {
                    Exit ();
                }));
            }
            if (setting.inputType.down) {
                data.tempInstance.Add (Fn._.AddPointerEvent (PointerEventType.onPressDown, (d) => {
                    Exit ();
                }));
            }
        }
    }
    public override void OnDisable_ () { }




    private void Exit () {
        data.actionIndex = 0;
        enabled = false;
    }

    [System.Serializable]
    public class InputType {
        public bool click = true;
        public bool down = true;
    }


}