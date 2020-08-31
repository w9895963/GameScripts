using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Input : MonoBehaviour {
    public Collider2D triggerBox;


    public InputType inputType = new InputType ();
    public ExitType exitType = new ExitType ();


    public List<Object> elist = new List<Object> (0);




    private void OnEnable () {
        if (triggerBox) {
            if (inputType.click) {
                var e = triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerClick, (d) => {
                    Exit ();
                });
                elist.Add (0, e);
            }
            if (inputType.down) {
                var e = triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerDown, (d) => {
                    Exit ();
                });
                elist.Add (1, e);
            }
        }
    }



    private void OnDisable () {
        elist.Destroy (0, 1);

    }

    private void Exit () {
        enabled = false;

        if (exitType.grab | exitType.auto) {
            I_Grab comp = GetComponent<I_Grab> ();
            if (comp) {
                comp.enabled = true;
            }
        }
        if (exitType.intoBackpack | exitType.auto) {
            var comp = GetComponent<I_InPack> ();
            if (comp) {
                comp.enabled = true;
            }
        }

    }

    [System.Serializable]
    public class InputType {
        public bool click = true;
        public bool down = true;
    }

    [System.Serializable]
    public class ExitType {
        public bool auto = false;
        public bool intoBackpack = false;
        public bool grab = false;
    }
}