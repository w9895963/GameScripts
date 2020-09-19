using System.Collections;
using System.Collections.Generic;
using Globle;
using UnityEngine;

public class C1_Follow : MonoBehaviour {
    [System.Serializable] public class Setting {
        public GameObject target;
        //*------------------------------
        public SnapToArea snapToArea = new SnapToArea ();
        public Lazy lazy = new Lazy ();
        //*--------------------------------
        [System.Serializable] public class SnapToArea {
            public bool enabled = false;
            public Collider2D target;

        }

        [System.Serializable] public class Lazy {
            public bool enabled = false;
            public float maxDistance = 1f;
            public float velosity = 1f;
        }
    }
    public Setting setting = new Setting ();
    //*--------------------
    public UpdateMethod updateMethod = default;
    //*--------------------------
    private bool initial = false;
    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();
    private Vector2 targetPosition;

    //*--------------------
    private void Start () {
        targetPosition = transform.position;
    }

    private void LateUpdate () {
        Vector2 nextP = targetPosition;

        if (setting.lazy.enabled) {
            Setting.Lazy vr = setting.lazy;
            Vector2 currP = (Vector2) transform.position;
            Vector2 tarP = (Vector2) targetPosition;
            Vector2 v = tarP - currP;
            float dist = v.magnitude;
            if (dist > 0) {
                float moveDist = vr.velosity * Time.deltaTime;
                if (dist > moveDist) {
                    nextP = currP + v.normalized * moveDist;
                } else {
                    nextP = tarP;
                }

                dist = (tarP - nextP).magnitude;
                if (dist > vr.maxDistance) {
                    nextP = tarP - vr.maxDistance * v.normalized;
                }

            }
        }




        if (nextP != (Vector2) transform.position)
            transform.Set2dPosition (nextP);




    }
    private void OnEnable () {
        Object tempObj;
        tempObj = setting.target.Ex_AddPositionEvent (updateMethod, (d) => {
            targetPosition = (Vector2) d.position;
            if (setting.snapToArea.enabled) {
                var targetP = setting.snapToArea.target;
                targetPosition = targetP.ClosestPoint (targetPosition);
            }

        });
        temps.Add (tempObj);
    }
    private void OnDisable () {
        temps.Destroy ();
    }


    private void Awake () {
        if (initial == false)
            enabled = false;
    }

    private void OnValidate () {
        initial = true;
    }




    //* Public Method
    public static C1_Follow Follow (GameObject gameObject, Setting setting) {
        C1_Follow comp = gameObject.gameObject.AddComponent<C1_Follow> ();
        comp.setting = setting;
        comp.enabled = true;
        return comp;
    }
}

public static class _Extention_Static {
    public static C1_Follow Follow (this GameObjectMethod source, C1_Follow.Setting setting) =>
        C1_Follow.Follow (source.gameObject, setting);
}