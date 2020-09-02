using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class I_Grab : IC_Base {
    [System.Serializable]
    public class Setting {
        public Rigidbody2D rigidBody = null;
        public float force = 80f;
        public AnimationCurve forceDistanceCurve = Fn.Curve.ZeroOneCurve;
        public float curveMaxDistance = 0.3f;
        public Vector2 pointForcePosition = default;




        public DistanceToTarget maxDistance = new DistanceToTarget ();
        public CharactorMove charactorMove = new CharactorMove ();
        [System.Serializable]
        public class DistanceToTarget {
            public bool enable = false;
            public GameObject target;
            public float distance = 1f;
        }

        [System.Serializable]
        public class CharactorMove {
            public bool enable = false;
            public M_PlayerMove playerMoveComp = null;
            public float speed = 3f;
            [ReadOnly] public float defaultSpeed;
            [ReadOnly] public Object moveEvent;
        }
    }

    [System.Serializable]
    public class Variables {
        public C1_TargetForce targetForceComp = null;
        public bool onGrab = false;
        public Vector2 targetPoint = Vector2.zero;
    }

    //************
    public Setting setting = new Setting ();
    [ReadOnly] public Variables variables = new Variables ();


    //********************
    public override void OnEnable_ () {
        StartGrab ();
    }

    public override void OnDisable_ () {
        StopGrab ();
    }
    private void FixedUpdate () {

        if (variables.onGrab) {
            if (setting.maxDistance.enable) {
                Vector2 position = (Vector2) setting.maxDistance.target.transform.position;
                Vector2 dir = variables.targetPoint - position;
                float value = Mathf.Min (dir.magnitude, setting.maxDistance.distance);
                variables.targetPoint = position + dir.normalized * value;
            }




            variables.targetForceComp.SetTarget (variables.targetPoint);
            Fn._.DrawPoint (variables.targetPoint);

        }


    }




    //* Private Method
    private void StartGrab () {
        variables.onGrab = true;
        variables.targetPoint = setting.rigidBody.position;
        if (setting.maxDistance.enable) {
            Vector2 position = (Vector2) setting.maxDistance.target.transform.position;
            variables.targetPoint = (setting.rigidBody.position - position).normalized * setting.maxDistance.distance + position;
        }
        Cursor.visible = false;

        if (!variables.targetForceComp) {
            variables.targetForceComp = setting.rigidBody.gameObject.Ex_AddTargetForce (
                variables.targetPoint,
                setting.force,
                setting.pointForcePosition,
                forceDistanceCurve : setting.forceDistanceCurve,
                curveMaxDistance : setting.curveMaxDistance,
                createBy : this);
        }


        SetupCharactorMoveEvent ();
        InputEventSet (true);
    }
    private void StopGrab () {
        variables.onGrab = false;
        Cursor.visible = true;
        variables.targetForceComp.Destroy ();


        DestroyCharactorMoveEvent ();
        InputEventSet (false);

        this.enabled = false;
    }


    private void InputEventSet (bool enabled) {
        if (enabled) {
            System.Func<Object> ev1 = () =>
                this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                    Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                    variables.targetPoint += vector;
                });
            data.tempInstance.AddIfEmpty (0, ev1);

            ev1 = () =>
                this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {
                    behaviour.actionIndex = 0;
                    StopGrab ();
                });
            data.tempInstance.AddIfEmpty (1, ev1);
        } else {
            data.tempInstance.Destroy (0, 1);
        }

    }
    private void SetupCharactorMoveEvent () {
        if (setting.charactorMove.enable) {
            if (setting.charactorMove.defaultSpeed == default) {
                setting.charactorMove.defaultSpeed = setting.charactorMove.playerMoveComp.maxSpeed;
            }
            setting.charactorMove.moveEvent.Destroy ();
            setting.charactorMove.moveEvent = this.Ex_AddMouseEvent (MouseEventType.onMove, (d) => {
                setting.charactorMove.playerMoveComp.Move (d.delta, setting.charactorMove.speed, 0.1f);
            });

        }

    }
    private void DestroyCharactorMoveEvent () {
        if (setting.charactorMove.moveEvent != null) {
            if (setting.charactorMove.playerMoveComp != null) {
                setting.charactorMove.playerMoveComp.maxSpeed = setting.charactorMove.defaultSpeed;
                setting.charactorMove.defaultSpeed = default;
            }
            setting.charactorMove.moveEvent.Destroy ();
        }

    }




}