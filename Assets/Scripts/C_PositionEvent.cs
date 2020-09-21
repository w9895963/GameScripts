using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;

public class C_PositionEvent : MonoBehaviour {
    [System.Serializable] public class Setting {
        public UpdateMethod updateMethod = UpdateMethod.FixedUpdate;
        public UnityEvent<PositionEventData> moving = new UnityEvent<PositionEventData> ();
        public UnityEvent<PositionEventData> stay = new UnityEvent<PositionEventData> ();
        public UnityEvent<PositionEventData> startMove = new UnityEvent<PositionEventData> ();
        public UnityEvent<PositionEventData> endMove = new UnityEvent<PositionEventData> ();
    }

    public Setting setting = new Setting ();

    //*----------------------
    private Vector3? lastPosition;
    private bool moving;

    //*---------------------------
    void Start () {
        lastPosition = transform.position;
    }

    void Update () {
        if (setting.updateMethod == UpdateMethod.Update) {
            MainUpdate ();
        }
    }

    void FixedUpdate () {
        if (setting.updateMethod == UpdateMethod.FixedUpdate) {
            MainUpdate ();
        }
    }

    void LateUpdate () {
        if (setting.updateMethod == UpdateMethod.LateUpdate) {
            MainUpdate ();
        }
    }



    //* Private Method
    private void MainUpdate () {
        Vector3 position = transform.position;
        Setting s = setting;

        PositionEventData evData = PositionEventData.Create ((d) => {
            d.position = position;
            d.lastPosition = lastPosition;
            d.UpdateMethod = s.updateMethod;
            d.deltaTime = (s.updateMethod == UpdateMethod.FixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime;
        });

        if (lastPosition != position) {
            evData.Moving = true;
            if (moving == false) {
                s.startMove.Invoke (evData);
            }
            moving = true;
            s.moving.Invoke (evData);
        } else {
            evData.Moving = false;
            if (moving == true) {
                s.endMove.Invoke (evData);
            }
            moving = false;
            s.stay.Invoke (evData);
        }

        lastPosition = position;
    }


}


public static class Extension_C_GameObjectEvent {
    public static C_PositionEvent AddPositionEvent (this GameObjectExMethod source,
        UnityAction<C_PositionEvent.Setting> setting = default
    ) { //--------------------------
        C_PositionEvent comp = source.gameObject.AddComponent<C_PositionEvent> ();
        setting (comp.setting);

        return comp;
    }
}

namespace Global {
    public enum UpdateMethod { Update, LateUpdate, FixedUpdate }


    public class PositionEventData {
        public Vector3 position;
        public Vector3? lastPosition;
        public float deltaTime;
        public bool Moving;
        public UpdateMethod UpdateMethod = default;

        public PositionEventData (UnityAction<PositionEventData> action) {

        }
        public static PositionEventData Create (UnityAction<PositionEventData> action) {
            PositionEventData ins = new PositionEventData ();
            action (ins);
            return ins;
        }

        public PositionEventData (
            Vector3 position,
            Vector3? lastPosition = null
        ) {
            this.position = position;
            this.lastPosition = lastPosition;
        }

        public PositionEventData () { }
    }

}