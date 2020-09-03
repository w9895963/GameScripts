using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class I_Grab : IC_ThreeWayInspector {
    [System.Serializable]
    public class Setting {
        public Rigidbody2D rigidBody = null;
        public RefFloat force = new RefFloat ();
        public AnimationCurve forceDistanceCurve = Fn.Curve.ZeroOneCurve;
        public float curveMaxDistance = 0.3f;
        public Vector2 pointForcePosition = default;




    }

    [System.Serializable]
    public class Variables {
        public C1_TargetForce targetForceComp = null;
        public RefVector2 targetPoint = new RefVector2 ();
    }

    //************
    public Setting setting = new Setting ();
    [ReadOnly] public Variables variables = new Variables ();


    //********************
    void OnEnable () {
        data.actionIndex = -1;
        StartGrab ();
    }

    void OnDisable () {
        Cursor.visible = true;
        variables.targetForceComp.Destroy ();


        InputEventSet (false);
        // StopGrab ();
    }
    private void FixedUpdate () {
        Vector2 targetPoint = variables.targetPoint.value;

        Fn._.DrawPoint (targetPoint);



    }




    //* Private Method
    private void StartGrab () {
        variables.targetPoint.value = setting.rigidBody.position;
        Cursor.visible = false;

        if (!variables.targetForceComp) {
            variables.targetForceComp =
                setting.rigidBody.gameObject.Ex_AddTargetForce (
                    variables.targetPoint.value,
                    setting.force.value,
                    setting.pointForcePosition,
                    forceDistanceCurve : setting.forceDistanceCurve,
                    curveMaxDistance : setting.curveMaxDistance,
                    createBy : this
                );
            variables.targetForceComp.Force = setting.force;
            variables.targetForceComp.TargetPosition = variables.targetPoint;
        }


        InputEventSet (true);
    }



    private void InputEventSet (bool enabled) {
        if (enabled) {
            System.Func<Object> ev1 = () =>
                this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                    Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                    variables.targetPoint.value += vector;
                });
            data.tempInstance.AddIfEmpty (0, ev1);

            ev1 = () =>
                this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {
                    data.actionIndex = 0;
                    this.enabled = false;
                });
            data.tempInstance.AddIfEmpty (1, ev1);
        } else {
            data.tempInstance.Destroy (0, 1);
        }

    }




}