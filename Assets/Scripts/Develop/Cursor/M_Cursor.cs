using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Global.Mods;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static Global.Function;
using UnityEngine.Events;

public class M_Cursor : MonoBehaviour, IModable, IModableSprite {

    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public bool hideSystemCursor = true;
        public bool autoHideSystemCursorNotEditor = true;
        public float scale = 0.2f;
        public List<State> states = new List<State> (4);
        [System.Serializable] public class State {
            public string stateName;
            public Sprite unpress;
            public Sprite pressed;

        }
    }
    public bool saveToMod = false;
    [ReadOnly] public List<Object> events = new List<Object> ();
    //* Public Property
    private Image image;
    public Image Image {
        get {
            if (image == null) {
                image = GetComponent<Image> ();
            }
            return image;
        }

    }
    private int stateIndex;
    public string State {
        set {
            stateIndex = setting.states.FindIndex ((x) => x.stateName == value);
            stateIndex = Mathf.Clamp (stateIndex, 0, setting.states.Count - 1);
        }
        get => setting.states[stateIndex].stateName;
    }
    //* Private Fields
    private Animator animator;
    private float scale = 0;
    private Sprite lastSprite;
    private bool presseDown;

    //* Basic Event Function
    private void Awake () { }
    private void Update () {
        Sprite currSprite = GetComponent<Image> ().sprite;
        if (lastSprite != currSprite) {
            UpdateSize ();
        }
        lastSprite = currSprite;
    }
    private void OnEnable () {
        var image = GetComponent<Image> ();
        animator = GetComponent<Animator> ();
        transform.localScale = setting.scale.ToVector2 ();
        UpdateSize ();
        image.raycastTarget = false;
        Cursor.visible = !setting.hideSystemCursor;
        if (!Application.isEditor & setting.autoHideSystemCursorNotEditor) {
            Cursor.visible = false;
        }

        Object pointerEventObj;

        pointerEventObj = Fn (this).AddGlobalPointerEvent (PointerEventType.onPressDown, (d) => {
            presseDown = true;
            UpdateCursorImage ();
        });
        events.Add (pointerEventObj);

        pointerEventObj = Global.Function.Fn (this).AddGlobalPointerEvent (PointerEventType.onPressUp, (d) => {
            presseDown = false;
            UpdateCursorImage ();
        });
        events.Add (pointerEventObj);


    }
    private void OnDisable () {
        events.Destroy ();
        Cursor.visible = true;
    }




    //* Private Method
    private void UpdateSize () {
        var image = GetComponent<Image> ();
        if (image.sprite) {
            var rectTr = GetComponent<RectTransform> ();
            float height = rectTr.sizeDelta.y;
            Rect imageSize = image.sprite.rect;
            if (scale == 0) {
                scale = height / imageSize.height;
            }
            height = imageSize.height * scale;

            float width = imageSize.width / imageSize.height * height;
            rectTr.sizeDelta = new Vector2 (width, (float) height);
            rectTr.pivot = new Vector2 (image.sprite.pivot.x / imageSize.width, image.sprite.pivot.y / imageSize.height);

        }

    }
    private void UpdateCursorImage () {
        Setting.State st = setting.states[stateIndex];
        if (presseDown) {
            Image.sprite = st.pressed;

        } else {
            Image.sprite = st.unpress;
        }
    }



    //* Interface Imodable
    public string ModTitle => "CustomCursor";
    public bool EnableWriteModDatas => saveToMod;
    public System.Object ModableObjectData => setting;

    public List<Sprite> ModableSprites {
        get =>
            setting.states.SelectMany ((x) => new List<Sprite> { x.unpress, x.pressed }).ToList ();
        set =>
            SetSprites (value);
    }
    public void LoadModData (ModData loader) {
        loader.LoadObjectDataTo<Setting> (setting);
        loader.LoadSprites ();

        UpdateCursorImage ();
    }


    private void SetSprites (List<Sprite> value) {
        for (int i = 0; i < value.Count; i++) {
            if (value[i] != null) {
                if (i % 2 == 0) setting.states[i / 2].unpress = value[i];
                else if (i % 2 == 1) setting.states[i / 2].pressed = value[i];
            }
        }
    }




}




namespace Global {
    public static class CursorCtrl {
        public static void ShowSystemCursor (bool enabled) {
            UnityEngine.Cursor.visible = enabled;
        }
    }

}