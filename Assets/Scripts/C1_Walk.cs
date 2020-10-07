using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

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
    private C0_Contact contactComponent;


    // * ---------------------------------- 


    private void FixedUpdate () {



    }

    private void OnEnable () {
        C0_Contact comp = GetComponent<C0_Contact> ();;
        if (!comp) {
            comp = gameObject.AddComponent<C0_Contact> ();
            comp.createBy = this;
        }
        comp.useBy.Add (this);

        comp.events.onChange.AddListener (Onchange);




        contactComponent = comp;
    }

    private void Onchange () {
        Debug.Log (contactComponent.ExistNormal (-MainCharacter.Gravity, 80));
    }

    private void OnDisable () {
        C0_Contact comp = contactComponent;
        comp.events.onChange.RemoveListener (Onchange);
        List<Object> useBy = comp.useBy;
        if (useBy.Count == 1 & useBy[0] == this) {
            comp.Destroy ();
        } else {
            useBy.Remove (this);
        }
    }



}