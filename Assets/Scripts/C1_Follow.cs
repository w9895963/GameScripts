using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class C1_Follow : MonoBehaviour {
    [System.Serializable] public class Setting {
        public UpdateMethod updateMethod = UpdateMethod.FixedUpdate;
        public GameObject target;
        //*------------------------------
        public Offset offset = new Offset ();
        public SnapToArea snapToArea = new SnapToArea ();
        public ConstantVelosity constantVelosity = new ConstantVelosity ();
        public DistanceConstraint distanceConstraint = new DistanceConstraint ();
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

        [System.Serializable] public class ConstantVelosity {
            public bool enabled = false;
            public float velosity = 1f;

        }

        [System.Serializable] public class DistanceConstraint {
            public bool enabled = false;
            public float maxDistance = 1f;
        }


    }
    public Setting setting = new Setting ();

    //*--------------------------
    private bool initial = false;
    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();
    private Vector2 targetObjectPosition;
    private Vector2 targetPosition;
    private Vector2 targetP;
    private bool moving;
    private Vector2 nextP;
    private Vector2 targetDelta;
    private List<System.Action> orderRun = new List<System.Action> ();
    private Vector2? lazyCurrP;
    private Vector2 lastP;

    //*--------------------
    private void Start () {
        targetP = setting.target.Get2dPosition ();
    }
    private void FixedUpdate () {
        var curr = lastP;
        nextP = targetP;
        Offset (ref nextP);
        Snap (ref nextP);
        Vector2 target = nextP;
        ConstantVelosity (lastP, ref nextP);
        if (setting.constantVelosity.enabled)
            curr = nextP;
        Lazy (curr, target, ref nextP);
    }



    private void LateUpdate () {
        if (nextP != null) {
            gameObject.Set2dPosition (nextP);
        } else {
            gameObject.Set2dPosition (targetP);
        }
        lastP = gameObject.Get2dPosition ();
    }

    private void OnEnable () {
        Object tempObj;
        tempObj = setting.target._ExMethod (this).AddPositionEvent ((x) => {
            x.updateMethod = setting.updateMethod;
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
                Vector2? point = targetColl._Ex (this).ClosestPointToLine (
                    nextP, direction
                );
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
    private void ConstantVelosity (Vector2 lastP, ref Vector2 nextP) {
        if (setting.constantVelosity.enabled) {
            var s = setting.constantVelosity;
            Vector2 target = nextP;
            Vector2 currP = lastP;
            Vector2 v = target - currP;
            float dist = v.magnitude;
            float nextMove = s.velosity * Time.fixedDeltaTime;
            if (dist > nextMove) {
                nextP = lastP + v.normalized * nextMove;
            } else {
                nextP = target;
            }
        }
    }
    private void Lazy (Vector2 lastP, Vector2 targetP, ref Vector2 nextP) {
        if (setting.distanceConstraint.enabled) {
            var s = setting.distanceConstraint;
            Vector2 tarP = targetP;
            Vector2 currP = lastP;
            Vector2 v = tarP - currP;
            float dist = v.magnitude;

            if (dist > s.maxDistance) {
                nextP = tarP - v.normalized * s.maxDistance;
            } else {
                nextP = lastP;
            }

        }
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
    public static C1_Follow Follow (this GameObjectExMethod source, C1_Follow.Setting setting) =>
        C1_Follow.Follow (source.gameObject, setting);
}