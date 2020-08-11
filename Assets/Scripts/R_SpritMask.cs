using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class R_SpritMask : MonoBehaviour {
    public GameObject background;
    [ReadOnly, SerializeField]
    private SpriteShapeController curve;
    void Start () {

        GameObject obj = GameObject.FindWithTag ("ClickableZone");


        Fn.AddEventToTrigger (obj, EventTriggerType.BeginDrag, (d) => {
            PointerEventData data = (PointerEventData) d;
            if (curve == null)
                curve = new GameObject ("DrawShape").AddComponent<SpriteShapeController> ();
            curve.splineDetail = 16;
            curve.spriteShapeRenderer.sortingOrder = 1;
            Vector2 p = Camera.main.ScreenToWorldPoint (data.position);
            curve.transform.position = p;
            curve.spline.Clear ();

        });



        var entry = Fn.AddEventToTrigger (obj, EventTriggerType.Drag, (d) => {
            PointerEventData data = (PointerEventData) d;
            Spline spline = curve.spline;
            Vector2 position = Camera.main.ScreenToWorldPoint (data.position) - curve.transform.position;


            spline.InsertPointAt (spline.GetPointCount (), position);
        });



        Fn.AddEventToTrigger (obj, EventTriggerType.EndDrag, (d) => {
            PointerEventData data = (PointerEventData) d;


            // int layerInt = curve.gameObject.layer;
            // curve.gameObject.layer = LayerMask.NameToLayer ("Temp");
            GameObject[] list = CutBackground (curve.gameObject, background, "Temp");

            background = list[0];

        });

    }

    private GameObject[] CutBackground (GameObject cutObject, GameObject background, string layerName) {
        int layerInt = curve.gameObject.layer;
        curve.gameObject.layer = LayerMask.NameToLayer (layerName);
        Bounds holeBounds = cutObject.gameObject.GetComponent<Renderer> ().bounds;
        GameObject mask = RenderLayerToMask (holeBounds, LayerMask.GetMask (layerName));

        Destroy (this.curve.gameObject);


        mask.layer = LayerMask.NameToLayer (layerName);
        background.layer = LayerMask.NameToLayer (layerName);
        Bounds backGrBounds = background.GetComponent<Renderer> ().bounds;

        MaskInteraction (background, SpriteMaskInteraction.VisibleOutsideMask);
        GameObject obj1 = RenderLayerToGameObject (backGrBounds, LayerMask.GetMask (layerName));
        // obj1.layer = layerInt;


        MaskInteraction (background, SpriteMaskInteraction.VisibleInsideMask);
        GameObject obj2 = RenderLayerToGameObject (holeBounds, LayerMask.GetMask (layerName));

        obj2.layer = layerInt;

        Destroy (background);
        Destroy (mask);

        // background = obj1;
        return new GameObject[] { obj1, obj2 };
    }

    public void MaskInteraction (GameObject gameObject, SpriteMaskInteraction intion) {
        if (gameObject.GetComponent<SpriteShapeRenderer> ())
            gameObject.GetComponent<SpriteShapeRenderer> ().maskInteraction = intion;
        else
            gameObject.GetComponent<SpriteRenderer> ().maskInteraction = intion;
    }




    public GameObject RenderLayerToMask (Bounds bounds, int layerMaks, int unitPixel = 100) {
        float re = unitPixel;
        Vector2 size = bounds.size;
        Vector3 center = bounds.center;
        float h = Mathf.Ceil (size.y * re);
        float w = Mathf.Ceil (size.x * re);


        Sprite sprite = renderLayerToSprite (bounds, layerMaks, unitPixel);


        GameObject mask = new GameObject ("Mask", typeof (SpriteMask));
        mask.GetComponent<SpriteMask> ().sprite = sprite;
        mask.transform.position = center;
        mask.transform.localScale = new Vector2 (h / re, h / re);
        return mask;
    }
    public GameObject RenderLayerToGameObject (Bounds bounds, int layerMaks, int unitPixel = 100) {
        float re = unitPixel;
        Vector2 size = bounds.size;
        Vector3 center = bounds.center;
        float h = Mathf.Ceil (size.y * re);
        float w = Mathf.Ceil (size.x * re);


        Sprite sprite = renderLayerToSprite (bounds, layerMaks, unitPixel);


        GameObject mask = new GameObject ("Sprite");
        mask.AddComponent<SpriteRenderer> ().sprite = sprite;
        mask.transform.position = center;
        mask.transform.localScale = new Vector2 (h / re, h / re);
        return mask;
    }




    public static Sprite renderLayerToSprite (Bounds bounds, int layerMaks, int unitPixel) {
        float re = unitPixel;
        Vector3 size = bounds.size;
        Vector3 center = bounds.center;
        float h = Mathf.Ceil (size.y * re);
        float w = Mathf.Ceil (size.x * re);



        RenderTexture te = new RenderTexture ((int) w, (int) h, 24);
        GameObject camera = new GameObject ("Cam", typeof (Camera));

        Camera cam = camera.GetComponent<Camera> ();
        cam.transform.position = new Vector3 (center.x, center.y, center.z - 10);
        cam.orthographic = true;
        cam.orthographicSize = h / re / 2;
        cam.targetTexture = te;
        cam.cullingMask = layerMaks;
        cam.Render ();



        Rect r = new Rect (0, 0, w, h);
        Texture2D t = new Texture2D ((int) w, (int) h);
        RenderTexture.active = te;
        t.ReadPixels (r, 0, 0);
        t.Apply ();
        RenderTexture.active = null;


        Destroy (camera);
        Destroy (te);
        return Sprite.Create (t, r, new Vector2 (0.5f, 0.5f), h);
    }
}