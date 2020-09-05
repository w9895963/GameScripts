using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class I_Grab : IC_FullInspector {

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
    public Setting setting = new Setting ();
    [ReadOnly] public Variables variables = new Variables ();


    //********************
    void OnEnable () {
        StartGrab ();
    }

    private void Update () {
        Vector2 targetPoint = variables.targetPoint;
        Setting.UnStable un = setting.unStabale;
        float scale = un.enable ? un.scale : 0;
        float distance = (setting.rigidBody.position - targetPoint).magnitude * un.scaleWithDistance;
        Vector2 vector2 = targetPoint + Random.insideUnitCircle * scale * distance;
        variables.targetPointOut.value = vector2;

        Fn._.DrawPoint (targetPoint);

    }




    //* Private Method
    private void StartGrab () {
        variables.targetPoint = setting.rigidBody.position;
        Cursor.visible = false;
        Object objAdd = null;
        if (!data.tempInstance.Has (2)) {
            var obj = setting.rigidBody.gameObject.Ex_AddTargetForce (
                variables.targetPoint,
                setting.force.value,
                setting.pointForcePosition,
                forceDistanceCurve : setting.forceDistanceCurve,
                curveMaxDistance : setting.curveMaxDistance,
                createBy : this
            );
            obj.Force = setting.force;
            obj.TargetPosition = variables.targetPointOut;
            objAdd = obj;
        }
        data.tempInstance.AddIfEmpty (2, objAdd);



        InputEventSet ();

        void InputEventSet () {
            var s = setting;

            System.Func<Object> ev1 = () =>
                this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                    Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                    variables.targetPoint += vector * s.tagetMoveScale;
                });
            data.tempInstance.AddIfEmpty (0, ev1);

            ev1 = () =>
                this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {
                    Cursor.visible = true;
                    this.enabled = false;
                    RunFinishedAction (0);
                });
            data.tempInstance.AddIfEmpty (1, ev1);




        }
    }




}