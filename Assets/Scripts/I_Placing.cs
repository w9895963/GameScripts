using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Placing : IC_Base {
    [System.Serializable] public class Setting {
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
       Global.Funtion.Fn(this).DrawLine (FindObjectOfType<M_PlayerManager> ().transform.position, setting.targetPosition, 0.01f);
    }
    void OnEnable () {


        data.tempInstance.AddIfEmpty (0,
            () =>Global.Funtion.Fn(this).AddPointerEvent (PointerEventType.onMoveNotDrag, (d) => {
                setting.targetPosition = d.position_Screen.ScreenToWold ();
            })
        );


        if (setting.endCondition.click) {
            data.tempInstance.AddIfEmpty (1,
                () =>Global.Funtion.Fn(this).AddPointerEvent (PointerEventType.onClick, (d) => {
                    enabled = false;
                })
            );
        }



    }
    void OnDisable () {

    }


}