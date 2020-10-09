using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;

public class C1_Follow : MonoBehaviour {
    [System.Serializable] public class Setting {
        public GameObject target;
        //*------------------------------
        public Offset offset = new Offset ();
        public SnapToArea snapToArea = new SnapToArea ();
        public LazyMode lazyMode = new LazyMode ();
        //*--------------------------------
        [System.Serializable] public class SnapToArea {
            public bool enabled = false;
            public Collider2D target;
            public Vector2 direction = default;

        }

        [System.Serializable] public class Offset {
            public bool enabled = false;
            public Vector2 offset = default;
        }

        [System.Serializable] public class LazyMode {
            public bool enabled = false;
            public float maxDistance = 1f;
            public float velosity = 1f;
            public float outRangeVelosity = 1000f;
        }


    }
    public Setting setting = new Setting ();

    //*--------------------------
    [ReadOnly] public Object callBy;
    private bool initial = false;
    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();
    private Vector2 targetP;
    private bool moving;
    private Vector2 nextP;
    private Vector2 lastP;

    //*--------------------
    private void FixedUpdate () {

        Vector2 curr = lastP;
        nextP = targetP;
        Offset (ref nextP);
        Snap (ref nextP);
        LazyMode (curr, ref nextP);
    }



    private void LateUpdate () {
        gameObject.SetPosition (nextP);
        lastP = gameObject.Get2dPosition ();
    }

    private void OnEnable () {
        targetP = setting.target.Get2dPosition ();
        nextP = gameObject.Get2dPosition ();
        lastP = gameObject.Get2dPosition ();
        Object tempObj;
        tempObj = setting.target._Ex (this).AddPositionEvent ((x) => {
            x.moving.AddListener ((d) => {
                moving = d.Moving;
                targetP = d.position;
            });
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

    //* Private Method

    private void Snap (ref Vector2 nextP) {
        if (setting.snapToArea.enabled) {
            Collider2D targetColl = setting.snapToArea.target;
            Vector2 direction = setting.snapToArea.direction;
            Vector2? result = null;

            if (direction != default) {
                Vector2? point = VectorHitPoint (targetColl.gameObject, nextP, direction);
                if (point.NotNull ())
                    result = point.ToVector2 ();
            }

            if (result.IsNull ()) {
                result = targetColl.ClosestPoint (nextP);
            }

            nextP = result.ToVector2 ();

        }
    }
    private void Offset (ref Vector2 camP) {
        if (setting.offset.enabled) {
            camP += setting.offset.offset;
        }
    }

    private void LazyMode (Vector2 lastP, ref Vector2 nextP) {
        if (setting.lazyMode.enabled) {
            var s = setting.lazyMode;
            Vector2 tarP = nextP;
            Vector2 currP = lastP;
            Vector2 v = tarP - currP;
            float dist = v.magnitude;

            if (dist > s.maxDistance) {
                float move = s.outRangeVelosity * Time.fixedDeltaTime;
                dist = (dist - move).ClampMin (s.maxDistance - 0.01f);
                nextP = tarP - v.normalized * dist;

            } else {
                var move = s.velosity * Time.fixedDeltaTime;
                if (move > dist) {
                    nextP = tarP;
                } else {
                    nextP = lastP + v.normalized * move;
                }
            }

        }
    }

    public Vector2? VectorHitPoint (GameObject gameObject, Vector2 position, Vector2 direction) {
        Vector2? result = null;
        int sourceLayer = gameObject.layer;
        gameObject.layer = Layer.tempLayer.Index;


        RaycastHit2D hit;
        hit = Physics2D.Raycast (position, direction, Mathf.Infinity, Layer.tempLayer.Mask);
        if (hit != default) {
            result = hit.point;
        } else {
            hit = Physics2D.Raycast (position, -direction, Mathf.Infinity, Layer.tempLayer.Mask);
            if (hit != default) {
                result = hit.point;
            }
        }

        gameObject.layer = sourceLayer;
        return result;
    }

}

public static class _Extention_Static {
    public static C1_Follow Follow (this GameObjectExMethod source,
        System.Func<C1_Follow.Setting, C1_Follow.Setting> setup
    ) {
        C1_Follow comp = source.gameObject.gameObject.AddComponent<C1_Follow> ();
        comp.setting = setup (comp.setting);
        comp.callBy = source.callby;



        comp.enabled = true;
        return comp;
    }
}