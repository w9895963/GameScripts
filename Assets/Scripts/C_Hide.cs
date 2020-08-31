using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Hide : MonoBehaviour {
    public Vector3 defaultPosition = default;
    public Object createBy;

    public void Hide () {
        defaultPosition = transform.position;
        gameObject.transform.position += new Vector3 (0, 0, -100);
    }
    public void Show () {
        transform.position = defaultPosition;
        this.Destroy ();
    }
}



public static class _Extension_C_Hide {
    public static C_Hide Ex_Hide (this GameObject gameObject, Object callBy = null) {
        C_Hide comp = gameObject.AddComponent<C_Hide> ();
        comp.Hide ();
        comp.createBy = callBy;
        return comp;
    }
    public static void Ex_Show (this GameObject gameObject) {
        C_Hide comp = gameObject.GetComponent<C_Hide> ();
        if (comp != null) {
            comp.Show ();
        }
    }
}