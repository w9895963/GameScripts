using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C0_TargetForce : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 target = default;
            public float force;
        }


    }
    private void FixedUpdate () {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();


        // rigidbody.AddForce();

    }



}