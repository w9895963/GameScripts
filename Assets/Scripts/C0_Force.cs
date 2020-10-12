using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using UnityEngine;
using UnityEngine.Events;

public class C0_Force : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        [System.Serializable] public class Basic {
            public Vector2 force;
        }
        public Basic basic = new Basic ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;

            public SpeedScaler speedScaler = new SpeedScaler ();
            [System.Serializable] public class SpeedScaler {
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
    // * ---------------------------------- 
    public Data data = new Data ();
    [System.Serializable] public class Data {
        public Vector2 realForce;
        public Vector2 velosity;
    }

    public Object createBy;
    public string label;




    // *MAIN

    private void FixedUpdate () {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D> ();
        Vector2 position = rigidBody.position;
        Vector2 forceAdd = setting.basic.force;




        if (setting.optional.speedScaler.enable) {
            var s = setting.optional.speedScaler;
            forceAdd *= s.curve.Evaluate (rigidBody.velocity.magnitude / s.maxSpeed);
        }



        if (setting.optional.ignoreMass)
            forceAdd *= rigidBody.mass;


        rigidBody.AddForce (forceAdd);




        data.velosity = rigidBody.velocity;
        data.realForce = forceAdd;

    }




}