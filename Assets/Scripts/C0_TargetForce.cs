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
        // * ---------------------------------- 
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;
        }

    }
    private void FixedUpdate () {
        Setting.Require require = setting.require;
        Setting.Optional optional = setting.optional;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
        Vector2 target = require.target;
        float force = require.force;
        Vector2 position = rigidbody.position;
        Vector2 vct = target - position;
        Vector2 forceAdd = force * vct.normalized;




        float massScale = optional.ignoreMass? rigidbody.mass : 1;
        rigidbody.AddForce (forceAdd * massScale);
    }



}