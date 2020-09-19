using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gb : MonoBehaviour {
    //*-----------------------Inspector
    public Behaviour behaviour = Behaviour.update;
    [SerializeField] public Data upload = new Data ();
    //*-----------------------Static
    public static Data _ = new Data ();




    private void Awake () {
        if (behaviour == Behaviour.update) {
            ApplyData ();
        }
    }


    private void OnValidate () {
        if (behaviour == Behaviour.update) {
            if (!UnityEditor.EditorApplication.isPlaying) {
                ApplyData ();
            }
        } else {
            if (upload != _) {
                upload = _;
            }
        }

    }


    //* Private Method
    private void ApplyData () {

        if (upload.backpack.obj) _.backpack.obj = upload.backpack.obj;
        if (upload.cursor) _.cursor = upload.cursor;
        if (upload.backpack.icon) _.backpack.icon = upload.backpack.icon;
        if (upload.render.indicateCamera) _.render.indicateCamera = upload.render.indicateCamera;

    }
    //* Class Definition
    public enum Behaviour { update, download }

    [System.Serializable] public class Data {
        public M_Cursor cursor;
        public M_PlayerManager mainCharactor;
        public Backpack backpack = new Backpack ();
        public Render render = new Render ();
        [System.Serializable] public class Render {
            public Camera indicateCamera;
        }

        [System.Serializable] public class Backpack {
            public M_BackPack obj;
            public GameObject icon;

        }

    }

}