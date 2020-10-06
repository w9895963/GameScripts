using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using System.Linq;
using UnityEngine;

public class C0_Contact : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public LayerFilter layerFilter = new LayerFilter ();
        [System.Serializable] public class LayerFilter {
            public bool enabled = false;
            public string layerName;
        }


    }
    public List<Contact> contacts = new List<Contact> ();

    [System.Serializable] public class Contact {
        public GameObject gameObject;
        public List<Vector2> normals;
    }


    private void OnDisable () {
        contacts.Clear ();
    }



    private void OnCollisionStay2D (Collision2D other) {
        if (enabled) {
            var gameObject = other.gameObject;
            var normals = other.contacts.ToList ().Select ((x) => x.normal).Distinct ().ToList ();

            if (LayerTest (other.gameObject)) {
                Contact contact = contacts.Find ((x) => x.gameObject == other.gameObject);
                if (contact != null) {
                    contact.normals = normals;
                } else {
                    contact = new Contact ();
                    contact.gameObject = gameObject;
                    contact.normals = normals;
                    contacts.Add (contact);
                }
            }
        }
    }

    private void OnCollisionExit2D (Collision2D other) {
        if (enabled) {
            contacts.RemoveAll ((x) => x.gameObject == other.gameObject);
        }
    }


    //* Private Method
    private bool LayerTest (GameObject obj) {
        if (setting.layerFilter.enabled) {
            if (obj.layer == LayerMask.NameToLayer (setting.layerFilter.layerName)) {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }

    }


}