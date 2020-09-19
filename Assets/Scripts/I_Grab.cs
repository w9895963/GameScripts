using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class I_Grab : IC_SmallCore {

    [System.Serializable] public class Variables {
        public Vector2 targetPoint = new Vector2 ();
        public RefVector2 targetPointOut = new RefVector2 ();
    }

    [System.Serializable] public class Setting {
        public Rigidbody2D rigidBody = null;
        public RefFloat force = new RefFloat (80);
        public AnimationCurve forceDistanceCurve = Fn.Curve.ZeroOneCurve;
        public float curveMaxDistance = 0.3f;
        public Vector2 pointForcePosition = default;
        public float tagetMoveScale = 1;


        [System.Serializable] public class UnStable {
            public bool enable = false;
            public float scale = 1;
            public float scaleWithDistance = 1;


        }

        [System.Serializable] public class ShareDate {
            public bool enable = false;
            public IC_Base shareTo;
            public string dataName;

        }
        public UnStable unStabale = new UnStable ();
        public ShareDate shareDate = new ShareDate ();




    }
    //************
    public Setting variable = new Setting ();
    [ReadOnly] public Variables middleData = new Variables ();


    //********************
    void OnEnable () {

        StartGrab ();

    }
    private void OnDisable () {

    }

    private void Update () {
        Vector2 targetPoint = middleData.targetPoint;
        Setting.UnStable un = variable.unStabale;
        float scale = un.enable ? un.scale : 0;
        float distance = (variable.rigidBody.position - targetPoint).magnitude * un.scaleWithDistance;
        Vector2 vector2 = targetPoint + Random.insideUnitCircle * scale * distance;
        middleData.targetPointOut.value = vector2;

        Fn._.DrawPoint (targetPoint);

    }



    //* Private Method
    private void StartGrab () {
        middleData.targetPoint = variable.rigidBody.position;
        Cursor.visible = false;
        Object objAdd = null;
        if (!data.tempInstance.Has (2)) {
            var obj = variable.rigidBody.gameObject.Ex_AddTargetForce (
                middleData.targetPoint,
                variable.force.value,
                variable.pointForcePosition,
                forceDistanceCurve : variable.forceDistanceCurve,
                curveMaxDistance : variable.curveMaxDistance,
                createBy : this
            );
            obj.Force = variable.force;
            obj.TargetPosition = middleData.targetPointOut;
            objAdd = obj;
        }
        data.tempInstance.AddIfEmpty (2, objAdd);



        InputEventSet ();

        void InputEventSet () {
            var s = variable;

            System.Func<Object> ev1 = () =>
                this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                    Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                    middleData.targetPoint += vector * s.tagetMoveScale;
                });
            data.tempInstance.AddIfEmpty (0, ev1);

            ev1 = () =>
                this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {
                    Cursor.visible = true;
                    this.enabled = false;
                });
            data.tempInstance.AddIfEmpty (1, ev1);




        }
    }

    //* Public Method
    public static I_Grab CreateComp (GameObject holder) {
        I_Grab comp = holder.AddComponent<I_Grab> ();


        return comp;
    }
}