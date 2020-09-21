using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_BackPack : MonoBehaviour {
    public I_InPack[] storage = new I_InPack[1];
    private EventTrigger iconEvent;

    private void OnEnable () {
        iconEvent.Destroy ();
        GameObject backpackIcon = Fn._.FindGlobalObject (GlobalObject.BackpackIcon);
        iconEvent = backpackIcon.Ex_AddInputToTrigger (EventTriggerType.PointerClick, (d) => {
            PutoutStorage (0);
        });
    }
    private void OnDisable () {
        iconEvent.Destroy ();
    }


    //* Public Method
    public void PutinStorage (I_InPack gameObject) {
        storage[0] = gameObject;

    }
    public void PutoutStorage (int index) {
        if (storage[index]) {
            storage[index].enabled = false;;
            storage[index] = null;
        }
    }
    public bool Contain (GameObject gameObject) {
        return storage[0] == gameObject;
    }
    public bool IsFull () {
        return storage[0] != null;
    }

}