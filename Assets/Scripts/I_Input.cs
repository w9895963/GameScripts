using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class I_Input : MonoBehaviour {
    public enum Action { globle, triggerBox }

    [System.Serializable] public class Setting {
        public Action action = Action.triggerBox;
        public Collider2D triggerBox;
        public bool inputOnece = true;
        public InputType inputType = new InputType ();
        [System.Serializable] public class InputType {
            public bool click = true;
            public bool down = false;
        }

    }
    public Setting setting = new Setting ();
    public Events events = new Events ();
    [System.Serializable] public class Events {
        public UnityEvent onInput = new UnityEvent ();

    }
    //********************
    public List<Object> tempObject = new List<Object> ();

    //************************

    void OnEnable () {
        Collider2D triggerBox = setting.triggerBox;
        if (setting.action == Action.triggerBox) {
            if (triggerBox) {
                if (setting.inputType.click) {
                    EventTrigger eventTrigger = triggerBox.Ex_AddInputToTrigger (EventTriggerType.PointerClick, (d) => {
                        SuccessInput ();
                    });
                    tempObject.Add (eventTrigger);
                }

                if (setting.inputType.down) {
                    tempObject.Add (
                        triggerBox.Ex_AddInputToTrigger (EventTriggerType.PointerDown, (d) => {
                            SuccessInput ();
                        })
                    );
                }
            }
        }
    }

    private void OnDisable () {
        tempObject.Destroy ();

    }

    private void SuccessInput () {
        if (setting.inputOnece) enabled = false;
        events.onInput.Invoke ();

    }
    //* Public Method
    public void AddEventOnInput (UnityAction action) {
        events.onInput.AddListener (action);
    }
    public static class CreateComp {
        public static I_Input Onece (GameObject gameObject,
            Collider2D clickBox = null
        ) {
            I_Input input = gameObject.AddComponent<I_Input> ();
            Setting varb = input.setting;


            if (clickBox) varb.triggerBox = clickBox;
            varb.inputOnece = true;


            // input.enabled = true;
            return input;
        }
        public static I_Input GblobleOnece (GameObject gameObject) {
            I_Input input = gameObject.AddComponent<I_Input> ();
            input.enabled = false;
            Setting varb = input.setting;
            varb.action = Action.globle;




            input.enabled = true;
            return input;
        }
    }


}