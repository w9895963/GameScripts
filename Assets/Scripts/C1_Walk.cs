using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
[RequireComponent (typeof (C0_Contact))]
public class C1_Walk : MonoBehaviour {
    public Setting setting = new Setting ();


    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 target;
            public float speed;
        }
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {

        }

    }

    private C0_Contact contactComp;


    // * ---------------------------------- 

    private void FixedUpdate () {



    }
    private void Awake () {
        contactComp = gameObject.GetComponent<C0_Contact> ();
    }
    private void OnEnable () {
        contactComp.events.onNormalChanged.AddListener (OnnNormalChanged);
    }
    private void OnDisable () {
        contactComp.events.onNormalChanged.RemoveListener (OnnNormalChanged);
    }




    private void OnnNormalChanged () {
        Debug.Log (contactComp.ExistNormal (-MainCharacter.Gravity, 80));
    }

}