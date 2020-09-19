using System.Collections;
using System.Collections.Generic;
using Globle;
using UnityEngine;
using UnityEngine.Events;

public class C_PositionEvent : MonoBehaviour {
    [System.Serializable] public class Setting {
        public UpdateMethod updateMethod = UpdateMethod.Update;
        public UnityEvent<PositionEventData> onPositionChanged = new UnityEvent<PositionEventData> ();

    }
    public Setting setting = new Setting ();

    //*----------------------
    private Vector3? lastLateUpdatePosition;
    private Vector3? lastUpdatePosition;
    private Vector3? lastFixedUpdatePosition;

    void Start () {
        lastLateUpdatePosition = transform.position;
        lastUpdatePosition = transform.position;
        lastFixedUpdatePosition = transform.position;
    }

    void Update () {
        if (setting.updateMethod == UpdateMethod.Update) {
            if (lastUpdatePosition != transform.position) {
                setting.onPositionChanged.Invoke (new PositionEventData (transform.position, lastUpdatePosition));
            }

            lastUpdatePosition = transform.position;
        }
    }
    private void FixedUpdate () {
        if (setting.updateMethod == UpdateMethod.FixedUpdate) {
            if (lastFixedUpdatePosition != transform.position) {
                setting.onPositionChanged.Invoke (new PositionEventData (transform.position, lastFixedUpdatePosition));
            }

            lastFixedUpdatePosition = transform.position;
        }
    }

    private void LateUpdate () {

        if (setting.updateMethod == UpdateMethod.LateUpdate) {
            if (lastLateUpdatePosition != transform.position) {
                setting.onPositionChanged.Invoke (new PositionEventData (transform.position, lastLateUpdatePosition));
            }

            lastLateUpdatePosition = transform.position;
        }
    }




}


public static class _Extionsiotn_C_GameObjectEvent {
    public static C_PositionEvent Ex_AddPositionEvent (this GameObject gameObject,
        UpdateMethod updateMethod,
        UnityAction<PositionEventData> action
    ) { //--------------------------
        C_PositionEvent comp = gameObject.AddComponent<C_PositionEvent> ();
        comp.setting.onPositionChanged.AddListener (action);
        comp.setting.updateMethod = updateMethod;

        return comp;
    }
}

namespace Globle {
    public enum UpdateMethod { Update, LateUpdate, FixedUpdate }


    public class PositionEventData {
        public Vector3 position;
        public Vector3? lastPosition;

        public PositionEventData (
            Vector3 position,
            Vector3? lastPosition = null
        ) {
            this.position = position;
            this.lastPosition = lastPosition;
        }
    }

}