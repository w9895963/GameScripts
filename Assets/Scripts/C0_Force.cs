using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using UnityEngine;
using UnityEngine.Events;

public class C0_Force : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        [System.Serializable] public class Require {
            public Vector2 force;
        }
        public Require require = new Require ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;

            public SpeedForceCurve speedForceCurve = new SpeedForceCurve ();
            [System.Serializable] public class SpeedForceCurve {
                public bool enable = false;
                public AnimationCurve curve = Global.Curve.OneZero;
                public float maxSpeed = 5f;

                public void Set (bool enable, AnimationCurve curve = default, float maxSpeed = 0) {
                    this.enable = enable;
                    this.curve = curve;
                    this.maxSpeed = maxSpeed;
                }
            }
        }
        public Optional optional = new Optional ();

    }


    public Object createby;




    // *MAIN
    private void FixedUpdate () {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D> ();
        Vector2 forceAdd = Vector2.zero;

        forceAdd += setting.require.force;



        if (setting.optional.speedForceCurve.enable) {
            var s = setting.optional.speedForceCurve;
            forceAdd *= s.curve.Evaluate (rigidBody.velocity.magnitude / s.maxSpeed);
        }



        if (setting.optional.ignoreMass) forceAdd *= rigidBody.mass;




        rigidBody.AddForce (forceAdd);

    }




    //*Public Method
    public Vector2 Force {
        set {
            setting.require.force = value;
        }
        get {
            return setting.require.force;
        }
    }


}


public static class Extension_C_Force {
    public static C0_Force AddForce (this Rigid2dExMethod source, UnityAction<C0_Force.Setting> setProfile) {
        C0_Force comp = source.rigidbody.gameObject.AddComponent<C0_Force> ();
        comp.createby = source.callby;
        setProfile (comp.setting);
        return comp;
    }
    public static C0_Force AddForce (this Rigid2dExMethod source,
        Vector2 force,
        float maxSpeed = default,
        AnimationCurve speedCurve = default

    ) {

        C0_Force comp = source.rigidbody.gameObject.AddComponent<C0_Force> ();
        comp.createby = source.callby;

        comp.setting.require.force = force;
        if (maxSpeed != default) {
            var curve = comp.setting.optional.speedForceCurve;
            curve.enable = true;
            curve.maxSpeed = maxSpeed;
            if (speedCurve != default) {
                curve.curve = speedCurve;
            }
        }

        return comp;
    }

}