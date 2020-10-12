using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.TargetForce;
using UnityEngine;

public class C0_TargetForce : MonoBehaviour {
    [SerializeField] private Profile setting = new Profile ();
    public Data data = new Data ();
    [System.Serializable] public class Data {
        public float speed;
        public Object creator;
        public string lable;
    }
    // * ---------------------------------- 
    private TargetForce core;

    private void Awake () {
        core = new TargetForce (setting, gameObject);
    }
    private void OnEnable () {

    }

    private void FixedUpdate () {
        core.ApplyForce ();


        data.speed = GetComponent<Rigidbody2D> ().velocity.magnitude;

    }


    public Profile Setting {
        get => core.vars;
        set {
            setting = value;
            core.vars = value;
        }
    }

}