using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Physic;
using static Global.Physic.PhysicUtility;
using System.Linq;
using Global.Mods;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMnager : MonoBehaviour, ILayer, IGravity, IPlayer, IModable {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Vector2 gravity = new Vector2 (0, -40);
        public float moveForce = 80;
        public float decelerate = 80;
        public float maxSpeed = 8;
        public float jumpForce = 200f;
        public float jumpLastTime;
        public AnimationCurve jumpCurve = Curve.OneZero;
    }
    public bool saveMod;
    public Vector2 Gravity => setting.gravity;
    private Vector2 moveButton;
    private float jumpButton;
    private Jump jump;
    private CollisionEvent collisionEvent;
    private Vector2 force;

    public int LayerIndex => LayerUtility.Lead.Index;

    public string ModTitle => ModUtility.GenerateTitle (this);

    public bool EnableWriteModDatas => saveMod;

    public object ModableObjectData => setting;

    private void Awake () {


        InputUtility.MoveInput.performed += (d) => {
            moveButton = d.ReadValue<Vector2> ();
        };
        InputUtility.JumpInput.performed += (d) => {
            jumpButton = d.ReadValue<float> ();

        };


    }

    private void OnEnable () {
        InputUtility.MoveInput.Enable ();
        InputUtility.JumpInput.Enable ();
        AddPhysicAction (gameObject, PhysicOrder.Movement, WalkAction);
        AddPhysicAction (gameObject, PhysicOrder.Jump, JumpAction);
        collisionEvent = AddColliderAction (gameObject, onStay : HitGround);


    }
    private void OnDisable () {
        InputUtility.MoveInput.Disable ();
        InputUtility.JumpInput.Disable ();
        RemovePhysicAction (gameObject, WalkAction);
        RemovePhysicAction (gameObject, JumpAction);
        collisionEvent.RemoveEvent ();
    }

    private void WalkAction (PhysicAction action) {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();
        Vector2 velosity = rigidbody.velocity;
        if (moveButton.x != 0) {
            float scaler = 1;
            if (velosity.x * moveButton.x >= 0) {
                scaler = Curve.Evaluate (velosity.x.Abs (), setting.maxSpeed * 0.5f, setting.maxSpeed, 1, 0);
            }
            force = new Vector2 (moveButton.x, 0) * setting.moveForce * scaler;
            action.SetForce (force);
        } else {
            float wantVx = 0;
            float currVx = velosity.x;
            float delx = wantVx - currVx;
            float deaccele = setting.decelerate * Time.fixedDeltaTime;
            float vChaned;
            if (delx.Abs () < deaccele) {
                vChaned = delx;
            } else {
                vChaned = deaccele * delx.Sign ();
            }
            action.SetForce (VelosityToForce(new Vector2(vChaned,0)));
        }
    }

    private void JumpAction (PhysicAction action) {
        if (jump == null) {
            if (jumpButton > 0) {
                jump = new Jump ();
                jump.beginTime = Time.time;
                jump.jumpCurve = setting.jumpCurve;
                jump.jumpLastTime = setting.jumpLastTime;
            }
        }

        if (jump != null) {
            action.SetForce (new Vector2 (0, jump.JumpScaler) * setting.jumpForce);
        }

    }
    private void HitGround (Collision2D other) {
        bool hitGround = other.contacts.Any ((x) => {
            return x.normal.Angle (Vector2.up) < 60;
        });
        if (hitGround) {
            jump = null;
        }

    }

    public void LoadModData (ModData data) {
        data.LoadObjectDataTo<Setting> (setting);
    }

    private class Jump {
        public float beginTime;
        public AnimationCurve jumpCurve;
        public float jumpLastTime;
        public float JumpScaler {
            get {
                float v = jumpCurve.Evaluate ((Time.time - beginTime) / jumpLastTime);
                return v;
            }
        }
    }
}