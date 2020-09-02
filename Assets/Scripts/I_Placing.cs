using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Placing : IC_Base {
    [System.Serializable] public class Setting {
        public Rigidbody2D targetBody;
        [ReadOnly] public Vector2 targetPosition;
        public int dataSlot = 0;
    }
    public Setting setting = new Setting ();



    void Update () {
        Fn._.DrawLineOnScreen (Gb.MainCharactor.transform.position, setting.targetPosition, 0.01f);
    }
    public override void EnableAction () {

        data.CallIfEmpty (0,
            () => Fn._.AddPointerEvent (PointerEventType.onMove, (d) => {
                setting.targetPosition = d.position_Screen.ScreenToWold ();
                data.shareData.Add (setting.dataSlot, setting.targetPosition);
            })
        );


        data.CallIfEmpty (1,
            () => Fn._.AddPointerEvent (PointerEventType.onClick, (d) => {
                enabled = false;
            })
        );



    }
    public override void DisableAction () {
        data.DestroyAllEvents ();




    }


}