using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Placing : IC_Base {
    [System.Serializable] public class Setting {
        public Rigidbody2D targetBody;
        public string ShareDataName = "PointerPosition";
        [ReadOnly] public Vector2 targetPosition;
        public EndCondition endCondition = new EndCondition ();



        //* Class Definition
        public class EndCondition {
            public bool click = true;


        }
    }
    public Setting setting = new Setting ();

    private void Reset () {
       
    }

    void Update () {
        Fn._.DrawLineOnScreen (Gb.MainCharactor.transform.position, setting.targetPosition, 0.01f);
    }
    public override void OnEnable_ () {


        data.tempInstance.AddIfEmpty (0,
            () => Fn._.AddPointerEvent (PointerEventType.onMove, (d) => {
                setting.targetPosition = d.position_Screen.ScreenToWold ();
                data.shareData.Add (setting.ShareDataName, setting.targetPosition);
            })
        );


        if (setting.endCondition.click) {
            data.tempInstance.AddIfEmpty (1,
                () => Fn._.AddPointerEvent (PointerEventType.onClick, (d) => {
                    enabled = false;
                })
            );
        }



    }
    public override void OnDisable_ () {
       
    }


}