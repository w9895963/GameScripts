using System.Collections;
using System.Collections.Generic;
using Global;
using static Global.Function;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class C0_Contact : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public bool useLayerFilter = true;
        public LayerUtility.PresetLayer layer = LayerUtility.PresetLayer.Obstacle;

    }
    public List<Contact> contacts = new List<Contact> ();

    [System.Serializable] public class Contact {
        public GameObject gameObject;
        public List<Vector2> normals;
    }
    public Events events = new Events ();
    [System.Serializable] public class Events {
        public UnityEvent onNormalChanged = new UnityEvent ();
    }
    // * ---------------------------------- 
    public Object createBy;
    public string tab;



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
                events.onNormalChanged.Invoke ();
            }
        }
    }

    private void OnCollisionExit2D (Collision2D other) {
        if (enabled) {
            contacts.RemoveAll ((x) => x.gameObject == other.gameObject);
            events.onNormalChanged.Invoke ();
        }
    }


    //* Private Method
    private bool LayerTest (GameObject obj) {
        if (setting.useLayerFilter) {
            if (obj.layer == setting.layer.ToLayer ().Index) {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }

    }
    //* Public Method


    public bool _ExistNormal (Vector2 normal, float angle) {
        return contacts.SelectMany ((x) => x.normals).ToList ().Exists ((x) => Vector2.Angle (x, normal) < angle);
    }
    public void _ApplyPreset (Preset preset) {
        if (preset == Preset.GroundTester) {
            setting.useLayerFilter = true;
            setting.layer = LayerUtility.PresetLayer.Obstacle;
        }
    }

    public enum Preset { GroundTester }
}