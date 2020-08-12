using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public bool runTeest;
    public RenderTexture te;
    // public Camera 
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    private void OnEnable () {

    }

    private void OnValidate () {
        if (runTeest) {
            runTeest = false;


            int w = 200;
            int h = 200;
            Rect r = new Rect (0, 0, w, h);
            Texture2D t = new Texture2D ((int) w, (int) h);
            RenderTexture.active = te;
            t.ReadPixels (r, 0, 0);
            t.Apply ();
            RenderTexture.active = null;

            var sprite = Sprite.Create (t, r, new Vector2 (0.5f, 0.5f), h);
            GameObject imageObject = new GameObject ("Sprite");
            imageObject.AddComponent<SpriteRenderer> ().sprite = sprite;
        }
    }
}