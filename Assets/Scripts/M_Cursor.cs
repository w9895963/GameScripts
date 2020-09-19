using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class M_Cursor : MonoBehaviour {
    public enum StateCs { normal, grab, walk }

    [SerializeField, ReadOnly] private StateCs state = StateCs.normal;
    //*----------------------Inspector
    public bool hideSystemCursor = true;
    [ReadOnly] public List<Object> events = new List<Object> ();
    //*-----------------------
    public static M_Cursor cursorObj;
    //*--------------------------
    private Animator animator;
    private float scale = 0;
    private Sprite sprite;


    //*------------------
    private void Awake () {
        if (cursorObj == null) cursorObj = this;
    }
    private void Update () {
        if (sprite != GetComponent<Image> ().sprite) {
            UpdateSize ();
        }
        sprite = GetComponent<Image> ().sprite;
    }
    private void OnEnable () {
        var image = GetComponent<Image> ();
        animator = GetComponent<Animator> ();

        UpdateSize ();
        image.raycastTarget = false;
        Cursor.visible = !hideSystemCursor;


        Object pointerEventObj = Fn._.AddPointerEvent (PointerEventType.onMove, (d) => {
            gameObject.GetComponent<RectTransform> ().position = d.position_Screen;
        });
        events.Add (pointerEventObj);

        pointerEventObj = Fn._.AddPointerEvent (PointerEventType.onPressDown, (d) => {
            animator.SetBool ("Click", true);
        });
        events.Add (pointerEventObj);

        pointerEventObj = Fn._.AddPointerEvent (PointerEventType.onPressUp, (d) => {
            animator.SetBool ("Click", false);
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


    //* Public Method
    public StateCs State {
        set {
            state = value;
            animator.SetInteger ("State", (int) value);
        }
        get => state;
    }



}