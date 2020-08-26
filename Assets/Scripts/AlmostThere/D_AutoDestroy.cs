using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AutoDestroy : MonoBehaviour {
    [SerializeField] private float time = 0;
    [SerializeField, ReadOnly] private float timeCreate = 0;
    public Component createBy = null;


    void Start () {
        timeCreate = Time.time;
    }

    void Update () {
        if (Time.time - timeCreate >= time) {
            Destroy (gameObject);
        }
    }

    private void OnEnable () {
        timeCreate = Time.time;
    }

    public void SetTime (float time) {
        this.time = time;
    }

    public static D_AutoDestroy AutoDestroy (GameObject gameObject, float time) {
        D_AutoDestroy au = gameObject.AddComponent<D_AutoDestroy> ();
        au.time = time;
        return au;
    }


}


public static class _Extension_D_AutoDestroy {
    public static void AutoDestroy (this Fn fn, GameObject gameObject, float time) =>
        D_AutoDestroy.AutoDestroy (gameObject, time);
}