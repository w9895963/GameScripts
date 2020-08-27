using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class M_Grab : MonoBehaviour {
    [Header ("Dependent Component")]
    public Rigidbody2D rigidBody = null;
    public Collider2D clickBox = null;
    [Header ("Setting")]
    public float force = 80f;
    public AnimationCurve forceDistanceCurve = Fn.Curve.OneOneCurve;
    public float curveMaxDistance = 1f;
    public Vector2 pointForcePosition = default;
    public TriggerZone triggerZone = new TriggerZone ();
    public DistanceToTarget maxDistance = new DistanceToTarget ();
    public CharactorMove charactorMove = new CharactorMove ();
    public Events events = new Events ();
    [SerializeField, ReadOnly] private C1_TargetForce targetForceComp = null;
    [SerializeField, ReadOnly] private bool onGrab = false;
    [SerializeField, ReadOnly] private Vector2 targetPoint = Vector2.zero;
    private Object[] inputEvents = new Object[7];

    private void OnDisable () {
        DestroyTriggerZoneEvent ();
        StopGrab ();
    }
    private void OnEnable () {
        SetupTriggerZoneEvent ();
    }

    private void FixedUpdate () {

        if (onGrab) {
            if (maxDistance.enable) {
                Vector2 position = (Vector2) maxDistance.target.transform.position;
                Vector2 dir = targetPoint - position;
                float value = Mathf.Min (dir.magnitude, maxDistance.distance);
                targetPoint = position + dir.normalized * value;
            }




            targetForceComp.SetTarget (targetPoint);
            Fn._.DrawPoint (targetPoint);

        }


    }




    //* Private Method
    private void StartGrab () {
        onGrab = true;
        targetPoint = rigidBody.position;
        if (maxDistance.enable) {
            Vector2 position = (Vector2) maxDistance.target.transform.position;
            targetPoint = (rigidBody.position - position).normalized * maxDistance.distance + position;
        }
        Cursor.visible = false;

        if (!targetForceComp) {
            targetForceComp = this.Ex_AddTargetForce (targetPoint,
                force,
                pointForcePosition,
                forceDistanceCurve : forceDistanceCurve,
                curveMaxDistance : curveMaxDistance);
        }


        events.grabBegin.Invoke ();
        SetupCharactorMoveEvent ();
        DestroyTriggerZoneEvent ();
    }
    private void StopGrab () {
        onGrab = false;
        Cursor.visible = true;
        targetForceComp.Destroy ();
        Fn._.Destroy (inputEvents);

        events.grabEnd.Invoke ();
        DestroyCharactorMoveEvent ();
        SetupTriggerZoneEvent ();
    }

    private void SetupInputEvent () {
        if (!inputEvents[0]) {

            EventTrigger eventTrigger = this.Ex_AddInputEventToTriggerOnece (clickBox, EventTriggerType.PointerClick, (d) => {
                StartGrab ();



                if (!inputEvents[1]) {
                    var ev = this.Ex_AddPointerEvent (PointerEventType.onDrag, (d2) => {
                        Vector2 vector = d2.position_Screen.ScreenToWold () - d2.lastPosition_Screen.ScreenToWold ();
                        targetPoint += vector;
                    });
                    inputEvents[1] = ev;
                }


                if (!inputEvents[2]) {
                    var ev1 = this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                        Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                        targetPoint += vector;
                    });
                    inputEvents[2] = ev1;
                }



                if (!inputEvents[3]) {
                    var ev2 = this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {

                        StopGrab ();
                        SetupInputEvent ();
                    });
                    inputEvents[3] = ev2;
                }

            });

            inputEvents[0] = eventTrigger;

        }




    }
    private void SetupTriggerZoneEvent () {
        if (triggerZone.enable & !triggerZone.eventObject) {
            triggerZone.eventObject = triggerZone.trigerZone.Ex_AddTriggerEvent (gameObject,
                (d) => {
                    SetupInputEvent ();
                }, (d) => {
                    Fn._.Destroy (inputEvents);
                });
        } else {
            SetupInputEvent ();
        }
    }
    private void DestroyTriggerZoneEvent () {
        triggerZone.eventObject.Destroy ();
    }
    private void SetupCharactorMoveEvent () {
        if (charactorMove.enable) {
            if (charactorMove.deafaltSpeed == default) {
                charactorMove.deafaltSpeed = charactorMove.playerMoveComp.maxSpeed;
            }
            charactorMove.moveEvent.Destroy ();
            charactorMove.moveEvent = this.Ex_AddMouseEvent (MouseEventType.onMove, (d) => {
                charactorMove.playerMoveComp.Move (d.delta, charactorMove.speed, 0.1f);
            });

        }

    }
    private void DestroyCharactorMoveEvent () {
        charactorMove.moveEvent.Destroy ();
        if (charactorMove.playerMoveComp != null) {
            charactorMove.playerMoveComp.maxSpeed = charactorMove.deafaltSpeed;
            charactorMove.deafaltSpeed = default;
        }

    }




    //*Property
    [System.Serializable]
    public class TriggerZone {
        public bool enable = false;
        public Collider2D trigerZone = null;
        [ReadOnly] public Object eventObject;


    }

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
        [ReadOnly] public float deafaltSpeed;
        [ReadOnly] public Object moveEvent;
    }

    [System.Serializable]
    public class Events {
        public UnityEvent grabBegin = new UnityEvent ();
        public UnityEvent grabEnd = new UnityEvent ();
    }

}