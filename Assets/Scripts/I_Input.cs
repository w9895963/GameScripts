using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Input : IC_SimpleInspector {
    public enum Action { globle, triggerBox }

    [System.Serializable] public class Setting {
        public Action target = Action.triggerBox;
        public Collider2D triggerBox;
        public InputType inputType = new InputType ();
        [System.Serializable] public class InputType {
            public bool click = true;
            public bool down = false;
        }

    }
    public Setting setting = new Setting ();




    public void OnEnable () {
        data.actionIndex = -1;
        Collider2D triggerBox = setting.triggerBox;
        if (setting.target == Action.triggerBox) {
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
        } else if (setting.target == Action.globle) {
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
    public void OnDisable () {

    }


    private void Exit () {
        data.actionIndex = 0;
        enabled = false;
    }




}