﻿using System.Collections;
using System.Collections.Generic;
using Global;
using Global.AttackSystem;
using Global.Physic;
using static Global.Physic.PhysicUtility;
using System.Linq;
using Global.Animate;
using Global.Mods;
using UnityEngine;
using static Global.Timer;
using UnityEngine.Events;

public class PlayerMnager : MonoBehaviour,  IGravity, IModable {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Vector2 gravity = new Vector2 (0, -40);
        public Move move = new Move ();
        [System.Serializable] public class Move : IModDataContainer {
            public float moveForce = 80;
            public float decelerate = 80;
            public float maxSpeed = 8;

        }
        public Jump jump = new Jump ();
        [System.Serializable] public class Jump : IModDataContainer {
            public float force = 200f;
            public float lastTime;
            public Vector2 JumpCurveProfile {
                get {
                    return Curve.CurveToVector2 (jumpCurve);
                }
                set {
                    Curve.SetCurveByVector2 (jumpCurve, value);
                }
            }
            public AnimationCurve jumpCurve = Curve.OneZero;

        }
        public Animation animation = new Animation ();
        [System.Serializable] public class Animation : IModDataContainer {
            public CharacterAnimation walking;
            public TextureAnimateProfile walkingTexture = new TextureAnimateProfile ();

            public CharacterAnimation standing;
            public TextureAnimateProfile standingTexture = new TextureAnimateProfile ();

        }
    }

    public bool saveMod;
    private Vector2 moveButton;
    private float jumpButton;
    private Jump jump;
    private CollisionEvent collisionEvent;
    private Vector2 force;
    private float lastMoveButtonX;
    private TimerControler animationTimer;
    private Vector2 faceDirection;

    public Vector2 Gravity => setting.gravity;
    public int LayerIndex => LayerUtility.Lead.Index;
    public Rigidbody2D RigidBody => GetComponent<Rigidbody2D> ();
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
        InputUtility.AttackInput.performed += (d) => {
            AttackUtility.Shoot (gameObject.GetPosition2d (), Vector2.Angle (Vector2.right, faceDirection));
            // Attack.Shoot ();
        };


    }

    private void OnEnable () {
        AddPhysicAction (gameObject, PhysicOrder.Movement, WalkAction);
        AddPhysicAction (gameObject, PhysicOrder.Jump, JumpAction);
        collisionEvent = AddColliderAction (gameObject, onStay : HitGround);


    }
    private void OnDisable () {
        RemovePhysicAction (gameObject, WalkAction);
        RemovePhysicAction (gameObject, JumpAction);
        collisionEvent.RemoveEvent ();
    }
    private void OnValidate () { }

    private void WalkAction (PhysicAction action) {
        ForceCalc ();
        void ForceCalc () {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();
            Vector2 velosity = rigidbody.velocity;
            if (moveButton.x != 0) {
                float scaler = 1;
                if (velosity.x * moveButton.x >= 0) {
                    scaler = Curve.Evaluate (velosity.x.Abs (), setting.move.maxSpeed * 0.5f, setting.move.maxSpeed, 1, 0);
                }
                force = new Vector2 (moveButton.x, 0) * setting.move.moveForce * scaler;
                action.SetForce (force);
            } else {
                float wantVx = 0;
                float currVx = velosity.x;
                float delx = wantVx - currVx;
                float deaccele = setting.move.decelerate * Time.fixedDeltaTime;
                float vChaned;
                if (delx.Abs () < deaccele) {
                    vChaned = delx;
                } else {
                    vChaned = deaccele * delx.Sign ();
                }
                action.SetForce (VelosityToForce (new Vector2 (vChaned, 0)));
            }
        }

        FaceDirection ();
        void FaceDirection () {
            if (moveButton.x != 0) {
                faceDirection = new Vector2 (moveButton.x, 0);
            }
        }



        bool animationChanged = moveButton.x != lastMoveButtonX;
        if (animationChanged) {
            if (animationTimer != null) animationTimer.Stop ();
            var holder = GetComponentInChildren<AnimationHolder> ();
            holder.DestroyChildren ();

            if (moveButton.x != 0) {
                GameObject obj = holder.CreateChild (setting.animation.walking.gameObject);
                bool v = (moveButton.x < 0) ? true : false;
                obj.WaitUpdate (() => {

                    animationTimer = AnimateUtility.SetTextureAnimate (obj, setting.animation.walkingTexture);
                    obj.GetComponent<SpriteRenderer> ().flipX = v;
                });


            } else {
                GameObject obj = holder.CreateChild (setting.animation.standing.gameObject);
                animationTimer = AnimateUtility.SetTextureAnimate (obj, setting.animation.standingTexture);
                bool v = (lastMoveButtonX < 0) ? true : false;
                obj.WaitUpdate (() => {
                    obj.GetComponent<SpriteRenderer> ().flipX = v;
                });
            }

        }
        lastMoveButtonX = moveButton.x;
    }

    private void JumpAction (PhysicAction action) {
        if (jump == null) {
            if (jumpButton > 0) {
                jump = new Jump ();
                jump.beginTime = Time.time;
                jump.jumpCurve = setting.jump.jumpCurve;
                jump.jumpLastTime = setting.jump.lastTime;
            }
        }

        if (jump != null) {
            action.SetForce (new Vector2 (0, jump.JumpScaler) * setting.jump.force);
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
    public void LoadModData (ModObjectData data) {
        data.LoadObjectDataTo<Setting> (setting);
    }

}