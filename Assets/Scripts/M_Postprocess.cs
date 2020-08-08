﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Postprocess : MonoBehaviour {
    public Camera mainCamera;
    public RenderTexture renderTexture;
    public GameObject secondScreen;
    public Test test;


    private void Start () {

        Main ();
    }


    private void Update () {
        Vector2 scale = secondScreen.transform.localScale;
        if (Screen.width / Screen.height != scale.x / scale.y) {
            Main ();
        }
    }
    private void Main () {
        if (!renderTexture) {
            RenderTextureDescriptor dis = new RenderTextureDescriptor (Screen.width, Screen.height);
            renderTexture = new RenderTexture (dis);
            mainCamera.GetComponent<Camera> ().targetTexture = renderTexture;
            secondScreen.GetComponent<SpriteRenderer> ().material.SetTexture ("_RenderTexture", renderTexture);
        }


        mainCamera.GetComponent<Camera> ().targetTexture = null;
        renderTexture.Release ();
        renderTexture.height = Screen.height;
        renderTexture.width = Screen.width;
        mainCamera.GetComponent<Camera> ().targetTexture = renderTexture;
        float scaleX = secondScreen.transform.localScale.y / Screen.height * Screen.width;
        Vector3 scale = secondScreen.transform.localScale;

        secondScreen.transform.localScale = new Vector3 (scaleX, scale.y, scale.z);


    }

    [System.Serializable]
    public class Test {
        public bool logResolution;
    }

    private void OnValidate () {
        if (test.logResolution == true) {
            Debug.Log (Screen.width);
            Debug.Log (Screen.height);
            test.logResolution = false;
        }
    }
}