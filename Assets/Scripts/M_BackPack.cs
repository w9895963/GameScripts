using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_BackPack : MonoBehaviour {
    public I_InPack[] storage = new I_InPack[1];
    // private EventTrigger iconEvent;
    private TempObject temp = new TempObject ();

    private void OnEnable () {
        // iconEvent.Destroy ();
        GameObject backpackIcon = GlobalObject.Get (GlobalObject.Type.BackpackIcon);
        temp.AddEventTrigger = backpackIcon._Ex (this).AddPointerEvent (EventTriggerType.PointerClick, (d) => {
            PutoutStorage (0);
        });
    }
    private void OnDisable () {
        // iconEvent.Destroy ();
        temp.DestroyAll ();
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