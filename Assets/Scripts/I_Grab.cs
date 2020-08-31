using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class I_Grab : MonoBehaviour {
    [Header ("Dependent Component")]
    public Rigidbody2D rigidBody = null;
    [Header ("Setting")]
    public float force = 80f;
    public AnimationCurve forceDistanceCurve = Fn.Curve.ZeroOneCurve;
    public float curveMaxDistance = 0.3f;
    public Vector2 pointForcePosition = default;
    public bool autoExit = true;




    public DistanceToTarget maxDistance = new DistanceToTarget ();
    public CharactorMove charactorMove = new CharactorMove ();
    public Events events = new Events ();




    [SerializeField, ReadOnly] private C1_TargetForce targetForceComp = null;
    [SerializeField, ReadOnly] private bool onGrab = false;
    [SerializeField, ReadOnly] private Vector2 targetPoint = Vector2.zero;
    private List<Object> elist = new List<Object> (7);




    private void OnDisable () {
        StopGrab ();
    }
    private void OnEnable () {
        StartGrab ();
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
            targetForceComp = rigidBody.gameObject.Ex_AddTargetForce (targetPoint,
                force,
                pointForcePosition,
                forceDistanceCurve : forceDistanceCurve,
                curveMaxDistance : curveMaxDistance,
                createBy : this);
        }


        SetupCharactorMoveEvent ();
        InputEventSet (true);
    }
    private void StopGrab () {
        onGrab = false;
        Cursor.visible = true;
        targetForceComp.Destroy ();


        DestroyCharactorMoveEvent ();
        InputEventSet (false);

        Exit ();
    }

    private void Exit () {
        events.exit.Invoke ();
        this.enabled = false;

        if (autoExit) {
            var e = GetComponent<I_Connecter> ();
            if (e)
                e.Enable ();
        }
    }

    private void InputEventSet (bool enabled) {
        if (enabled) {
            var ev1 = this.Ex_AddMouseEvent (MouseEventType.onMove, (d2) => {
                Vector2 vector = d2.delta.ScreenToWold () - Vector2.zero.ScreenToWold ();
                targetPoint += vector;
            });
            elist.Add (0, ev1);

            var ev2 = this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d2) => {
                StopGrab ();
            });
            elist.Add (1, ev2);
        } else {
            elist[0].Destroy ();
            elist[1].Destroy ();
        }

    }
    private void SetupCharactorMoveEvent () {
        if (charactorMove.enable) {
            if (charactorMove.defaultSpeed == default) {
                charactorMove.defaultSpeed = charactorMove.playerMoveComp.maxSpeed;
            }
            charactorMove.moveEvent.Destroy ();
            charactorMove.moveEvent = this.Ex_AddMouseEvent (MouseEventType.onMove, (d) => {
                charactorMove.playerMoveComp.Move (d.delta, charactorMove.speed, 0.1f);
            });

        }

    }
    private void DestroyCharactorMoveEvent () {
        if (charactorMove.moveEvent != null) {
            if (charactorMove.playerMoveComp != null) {
                charactorMove.playerMoveComp.maxSpeed = charactorMove.defaultSpeed;
                charactorMove.defaultSpeed = default;
            }
            charactorMove.moveEvent.Destroy ();
        }

    }




    //*Property

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

    [System.Serializable]
    public class Events {
        public UnityEvent exit = new UnityEvent ();
    }

}