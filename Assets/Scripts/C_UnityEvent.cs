using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_UnityDefaultEvent : MonoBehaviour {
    public UnityEvent onDestroy = new UnityEvent ();


    private void OnDestroy () {
        onDestroy.Invoke ();
    }
}


public static class _Extension_C_UnityEvent {
    public static C_UnityDefaultEvent Ex_AddDestroyEvent (this GameObject obj, UnityAction onDestroy) {
        C_UnityDefaultEvent comp = obj.AddComponent<C_UnityDefaultEvent> ();
        comp.onDestroy.AddListener (onDestroy);
        return comp;

    }
}