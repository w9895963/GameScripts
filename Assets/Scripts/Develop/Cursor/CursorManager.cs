using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Visible;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {
    public Texture2D cursorImage;
    public float scale = 0.1f;

    void Start () {
        RawImage cursor = GetComponentInChildren<RawImage> ();

        Cursor.visible = Application.isEditor?true : false;


        cursor.texture = cursorImage;
        cursor.SetNativeSize ();
        cursor.transform.localScale = new Vector2 (scale, scale);


        InputUtility.Pointer.performed += (d) => {
            var position = d.ReadValue<Vector2> ();
            cursor.transform.position = position;
        };
    }
}