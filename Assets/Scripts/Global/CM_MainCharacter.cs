using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Global.Funtion;
using Global;
using UnityEngine.Events;

public class CM_MainCharacter : MonoBehaviour {

    public WalkSetting walkSetting = new WalkSetting ();
    [System.Serializable] public class WalkSetting {
        public bool enabled = false;
        public Basic basic = new Basic ();
        [System.Serializable] public class Basic {
            public floatRef speed = new floatRef (6);
            public Collider2D inputZone;
        }
        public Advance advance = new Advance ();
        [System.Serializable] public class Advance {
            public float arriveTriggerDistance = 0.05f;
            public TargetForce.Profile.Optional.PIDSetting pIDSetting =
                new TargetForce.Profile.Optional.PIDSetting ();
            public TargetForce.Profile.Optional.VelosityControl VelosityControl =
                new TargetForce.Profile.Optional.VelosityControl ();

        }
    }
    // * ---------------------------------- 
    public Gravity gravity = new Gravity ();
    [System.Serializable] public class Gravity {
        public bool enabled = true;
        public Vector2 gravity = new Vector2 (0, -40);

        public C0_Force gravityComponent;
    }
    // * ---------------------------------- 
    public TempObject temp = new TempObject ();
    public C0_TargetForce walkingforce;
    private C_OnArrive arriveComp;

    private void Awake () {

    }

    private void OnEnable () {
        if (gravity.enabled) {
            gravity.gravityComponent = gameObject.AddComponent<C0_Force> ();
            temp.AddObject = gravity.gravityComponent;
            gravity.gravityComponent.createBy = this;
            gravity.gravityComponent.label = "Gravity";
            gravity.gravityComponent.setting.badic.force = gravity.gravity;
        }


        if (walkSetting.enabled) {
            AddComponent ();
            void AddComponent () {
                TargetForce.Profile profile = new TargetForce.Profile ();
                TargetForce.Profile.Optional optional = profile.optional;
                optional.enablePids = true;
                optional.enableVelosityControl = true;
                optional.velosityControl = walkSetting.advance.VelosityControl;
                walkSetting.advance.VelosityControl.maxSpeed = walkSetting.basic.speed;
                optional.enableSingleDimension = true;
                optional.singleDimension.dimensiion = MainCharacter.Gravity.Rotate (90);

                walkingforce = gameObject.AddComponent<C0_TargetForce> ();
                temp.AddObject = walkingforce;
                walkingforce.enabled = false;
                walkingforce.Setting = profile;
                walkingforce.data.creator = this;
                walkingforce.data.lable = "walkingforce";
            }
            temp.AddEventTrigger = walkSetting.basic.inputZone._Ex (this)
                .AddPointerEvent (EventTriggerType.PointerClick, (d) => {
                    Vector2 target = (d as PointerEventData).position.ScreenToWold ();
                    walkingforce.Setting.basic.target.v = target;
                    walkingforce.enabled = true;

                    Arrive (() => {
                        walkingforce.enabled = false;
                    });
                    void Arrive (UnityAction callback) {
                        Vector2 dir = MainCharacter.Gravity.Rotate (90);
                        arriveComp.Destroy ();
                        var distance = walkSetting.advance.arriveTriggerDistance;
                        arriveComp = Fn (this).OnArrive (gameObject, target, dir, distance, true, callback);
                        temp.AddObject = arriveComp;
                    }
                });
        }




    }

    private void OnDisable () {
        temp.DestroyAll ();
    }



}



namespace Global {
    public static class MainCharacter {
        private static CM_MainCharacter comp;
        public static CM_MainCharacter Comp {
            get {
                if (!comp) {
                    comp = GameObject.FindObjectOfType<CM_MainCharacter> ();
                }
                return comp;
            }
        }
        public static void ReverseGravity () {
            Comp.gravity.gravityComponent.setting.badic.force *= -1;
        }
        public static Vector2 Gravity => Comp.gravity.gravityComponent.setting.badic.force;
    }
}