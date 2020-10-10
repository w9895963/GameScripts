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

public class M_Cursor : MonoBehaviour {
    public enum StateCs { normal, grab, walk, manipulate }

    [SerializeField, ReadOnly] private StateCs state = StateCs.normal;
    //*----------------------Inspector
    public float sensitivity = 1;
    public bool hideSystemCursor = true;
    [ReadOnly] public List<Object> events = new List<Object> ();
    //*-----------------------
    private Animator animator;
    private float scale = 0;
    private Sprite lastSprite;
    public GameObject currentOverObject;
    public List<GameObject> overobjects = new List<GameObject> ();


    //*------------------
    private void Update () {
        Sprite currSprite = GetComponent<Image> ().sprite;
        if (lastSprite != currSprite) {
            UpdateSize ();
        }
        lastSprite = currSprite;



        #region //*Custom Pointer
        // Vector2 mouseDelta = Mouse.current.delta.ReadValue ();
        // if (mouseDelta.x != 0 | mouseDelta.y != 0) {
        //     transform.position += mouseDelta.ToVector3 () * sensitivity;
        //     Vector2 p = transform.position;

        //     gameObject.Set2dPosition (new Vector2 (p.x.Clamp (0, Screen.width), p.y.Clamp (0, Screen.height)));

        // }

        // PointerEventData pointerData = new PointerEventData (EventSystem.current);
        // pointerData.position = gameObject.Get2dPosition ();


        // List<RaycastResult> result = new List<RaycastResult> ();
        // EventSystem.current.RaycastAll (pointerData, result);

        // GameObject lastObj = currentOverObject;
        // GameObject currObj = (result.Count > 0) ? result[0].gameObject : null;


        // if (currObj != lastObj) {
        //     if (currObj != null) {
        //         List<GameObject> currOverList = currObj.GetComponentsInParent<Transform> ().ToList ()
        //             .Select ((x) => x.gameObject).ToList ();
        //         currOverList.ForEach ((x) => {
        //             if (!overobjects.Contains (x)) {
        //                 x.GetComponents<EventTrigger> ().ForEach ((y) => { y.OnPointerEnter (pointerData); });
        //                 overobjects.Add (x);
        //             }
        //         });
        //         overobjects.Except (currOverList).ToList ().ForEach ((x) => {
        //             x.GetComponents<EventTrigger> ().ForEach ((y) => { y.OnPointerExit (pointerData); });
        //             overobjects.Remove (x);
        //         });
        //     } else {
        //         overobjects.FindAll (x => true).ForEach ((x) => {
        //             x.GetComponents<EventTrigger> ().ForEach ((y) => {
        //                 y.OnPointerExit (pointerData);
        //             });
        //             overobjects.Remove (x);
        //         });

        //     }
        // }




        // currentOverObject = currObj;
        #endregion




    }
    private void OnEnable () {
        var image = GetComponent<Image> ();
        animator = GetComponent<Animator> ();

        UpdateSize ();
        image.raycastTarget = false;
        Cursor.visible = !hideSystemCursor;

        Object pointerEventObj;
        pointerEventObj = Fn (this).AddGlobalPointerEvent (PointerEventType.onMove, (d) => {
            gameObject.GetComponent<RectTransform> ().position = d.position_Screen;
        });
        events.Add (pointerEventObj);

        pointerEventObj = Fn (this).AddGlobalPointerEvent (PointerEventType.onPressDown, (d) => {
            animator.SetBool ("Click", true);
        });
        events.Add (pointerEventObj);

        pointerEventObj = Global.Funtion.Fn (this).AddGlobalPointerEvent (PointerEventType.onPressUp, (d) => {
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


namespace Global {
    public static class CursorCtrl {
        public static void ShowSystemCursor (bool enabled) {
            UnityEngine.Cursor.visible = enabled;
        }
    }

}