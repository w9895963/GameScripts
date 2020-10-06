using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class C1_Walk : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 direction;
            public float speed;
        }
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {

        }

    }
    private void FixedUpdate () {




    }



}