using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Funtion;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
    public Events events = new Events ();
    [System.Serializable] public class Events {
        public UnityEvent onChange = new UnityEvent ();
    }
    // * ---------------------------------- 
    public Object createBy;
    public List<Object> useBy = new List<Object> ();



    // * ---------------------------------- 
    private void OnDisable () {
        contacts.Clear ();
    }



    private void OnCollisionStay2D (Collision2D other) {
        if (enabled) {
            bool changed = false;
            var gameObject = other.gameObject;
            var normals = other.contacts.ToList ().Select ((x) => x.normal).Distinct ().ToList ();

            if (LayerTest (other.gameObject)) {
                Contact contact = contacts.Find ((x) => x.gameObject == other.gameObject);
                if (contact != null) {
                    if (!contact.normals.IsSame (normals)) {
                        changed = true;
                    }
                    contact.normals = normals;
                } else {
                    changed = true;
                    contact = new Contact ();
                    contact.gameObject = gameObject;
                    contact.normals = normals;
                    contacts.Add (contact);
                }
            }
            if (changed) {
                events.onChange.Invoke ();
            }
        }
    }

    private void OnCollisionExit2D (Collision2D other) {
        if (enabled) {
            contacts.RemoveAll ((x) => x.gameObject == other.gameObject);
            events.onChange.Invoke ();
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
    //* Public Method
    public bool ExistNormal (Vector2 normal, float angle) {
        return contacts.SelectMany ((x) => x.normals).ToList ().Exists ((x) => Vector2.Angle (x, normal) < angle);
    }
    public void ApplyPreset (Preset preset) {
        if (preset == Preset.GroundTester) {
            var layerFilter = setting.layerFilter;
            layerFilter.enabled = true;
            layerFilter.layerName = Layer.staticSolid.Name;

        }
    }

    public enum Preset { GroundTester }
}