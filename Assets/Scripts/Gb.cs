using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gb : MonoBehaviour {
    public enum Behaviour { update, download }
    public Behaviour behaviour = Behaviour.update;
    public static Data _ = new Data ();
    [SerializeField] public Data data = new Data ();
    [System.Serializable] public class Data {
        public Backpack backpack = new Backpack ();
        public Render render = new Render ();
        [System.Serializable] public class Render {
            public Camera indicateCamera;
        }

        [System.Serializable] public class Pointer {
            public Collider2D movingZone;
        }

        [System.Serializable] public class Backpack {
            public M_BackPack obj;
            public GameObject icon;

        }

    }
    public M_PlayerManager mainCharactor;
    public static M_PlayerManager MainCharactor;


    public Text screenLog;
    public static Text ScreenLog;



    public Image canvasBackGround;
    public static Image CanvasBackGround;
    public Image canvasTopLayer;
    public static Image CanvasTopLayer;




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
            if (data != _) {
                data = _;
            }
        }

    }


    //* Private Method
    private void ApplyData () {

        MainCharactor = mainCharactor ? mainCharactor : MainCharactor;
        ScreenLog = screenLog ? screenLog : ScreenLog;
        CanvasBackGround = canvasBackGround ? canvasBackGround : CanvasBackGround;
        CanvasTopLayer = canvasTopLayer ? canvasTopLayer : CanvasTopLayer;


        if (data.backpack.obj) _.backpack.obj = data.backpack.obj;
        if (data.backpack.icon) _.backpack.icon = data.backpack.icon;
        if (data.render.indicateCamera) _.render.indicateCamera = data.render.indicateCamera;

    }



}