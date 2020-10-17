using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static Global.Funtion;

public class M_Cursor : InterfaceHolder, IModable {

    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public bool hideSystemCursor = true;
        public float scale = 0.2f;
        public List<State> states = new List<State> (4);
        [System.Serializable] public class State {
            public string stateName;
            public Sprite unpress;
            public string unpress_SpriteData;
            public ModUtility.ModImage unpressModed = new ModUtility.ModImage ();
            public Sprite pressed;
            public string pressed_SpriteData;
            public ModUtility.ModImage pressedModed = new ModUtility.ModImage ();

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

    //*------------------
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

        Object pointerEventObj;

        pointerEventObj = Fn (this).AddGlobalPointerEvent (PointerEventType.onPressDown, (d) => {
            presseDown = true;
            UpdateCursorImage ();
        });
        events.Add (pointerEventObj);

        pointerEventObj = Global.Funtion.Fn (this).AddGlobalPointerEvent (PointerEventType.onPressUp, (d) => {
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
    public string ModDataName => "CustomCursor";
    public bool EnableWriteModDatas => saveToMod;
    public string ModData {
        get {
            List<System.Object> list = new List<object> ();
            list.Add (setting);
            var sprites = setting.states.SelectMany ((s) =>
                new List<Sprite> { s.pressed, s.unpress }).ToList ();

            var spDatas = sprites.Select ((x) => ModUtility.SpriteToSpritedate (x)).ToList ();
            list.AddRange (spDatas);

            return ModUtility.ToStoreData (list.ToArray ());
        }
    }
    public void LoadModData (string modData) {
        var sprites = setting.states.SelectMany ((s) =>
            new List<Sprite> { s.pressed, s.unpress }).ToList ();

        var list = ModUtility.FomeStoreData (modData);
        JsonUtility.FromJsonOverwrite (list[0], setting);




        List<string> lists = list.GetRange (1, list.Count - 1);
        Debug.Log (lists.Count);

        for (int i = 0; i < lists.Count; i++) {
            string v = lists[i];
            var spriteData = JsonUtility.FromJson<ModUtility.SpriteData> (v);
            if (spriteData != null) {
                spriteData.LoadSprite ();
                if (i % 2 == 0) {
                    setting.states[i / 2].pressed = spriteData.spriteObject;
                } else {
                    setting.states[i / 2].unpress = spriteData.spriteObject;
                }
            }

        }

        var sprites2 = setting.states.SelectMany ((s) =>
            new List<Sprite> { s.pressed, s.unpress }).ToList ();
        for (int i = 0; i < sprites2.Count; i++) {
            if (sprites2[i] == null) {
                if (i % 2 == 0) {
                    setting.states[i / 2].pressed = sprites[i];
                } else {
                    setting.states[i / 2].unpress = sprites[i];
                }

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