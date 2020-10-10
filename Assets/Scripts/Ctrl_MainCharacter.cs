using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Global.Funtion;
using Global;

public class Ctrl_MainCharacter : MonoBehaviour {

    public Walking walking = new Walking ();
    [System.Serializable] public class Walking {
        public bool enabled = false;
        public float speed = 6;
        public Collider2D inputZone;
    }
    // * ---------------------------------- 
    public Gravity gravity = new Gravity ();
    [System.Serializable] public class Gravity {
        public bool enabled = true;
        public Vector2 gravity = new Vector2 (0, -40);

    }
    // * ---------------------------------- 
    public C0_Force gravityComponent;
    public C0_Contact contactComponent;
    public TempObject temp = new TempObject ();
    private C0_DynimicTargetForce targetForce;

    private void Awake () { }

    private void OnEnable () {
        if (gravity.enabled) {
            gravityComponent = gameObject.AddComponent<C0_Force> ();
            gravityComponent.createBy = this;
            gravityComponent.label = "Gravity";
            gravityComponent.setting.badic.force = gravity.gravity;
        }


        if (walking.enabled) {
            contactComponent = gameObject.AddComponent<C0_Contact> ();
            temp.AddEventTrigger = walking.inputZone.gameObject._Ex (this)
                .AddPointerEvent (EventTriggerType.PointerClick, (d) => {
                    Vector2 p = (d as PointerEventData).position.ScreenToWold ();
                    if (!targetForce) {
                        targetForce = gameObject.AddComponent<C0_DynimicTargetForce> ();
                        targetForce.creator = this;
                        targetForce.lable = "Walking Force";
                        var setting = targetForce.setting;
                        setting.require.maxForce = 90;
                        var velosityControl = setting.optional.velosityControl;
                        velosityControl.enabled = true;
                        velosityControl.slowDownDistance = 0.6f;;
                        velosityControl.maxSpeed = walking.speed;
                        var singleDimension = setting.optional.singleDimension;
                        singleDimension.enabled = true;
                        singleDimension.dimensiion = MainCharacter.Gravity.Rotate (90);
                    }
                    targetForce.setting.require.target = p;


                });

        }
    }

    private void OnDisable () {
        gravityComponent.Destroy ();
        temp.DestroyAll ();
    }




}



namespace Global {
    public static class MainCharacter {
        private static Ctrl_MainCharacter comp;
        public static Ctrl_MainCharacter Comp {
            get {
                if (!comp) {
                    comp = GameObject.FindObjectOfType<Ctrl_MainCharacter> ();
                }
                return comp;
            }
        }
        public static void ReverseGravity () {
            Comp.gravityComponent.setting.badic.force *= -1;
        }
        public static Vector2 Gravity => Comp.gravityComponent.setting.badic.force;
    }
}