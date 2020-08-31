using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Input : IC_Base {
    public Collider2D triggerBox;

    public InputType inputType = new InputType ();






    public override void EnableAction () {
        if (triggerBox) {
            if (inputType.click) {
                data.CallEventIfEmpty (0, () =>
                    triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerClick, (d) => {
                        Exit ();
                    })
                );
            }

            if (inputType.down) {
                data.CallEventIfEmpty (1, () =>
                    triggerBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerDown, (d) => {
                        Exit ();
                    })
                );
            }
        }
    }
    public override void DisableAction () {
        data.DestroyEvents (0, 1);
    }




    private void Exit () => enabled = false;

    [System.Serializable]
    public class InputType {
        public bool click = true;
        public bool down = true;
    }


}