using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
using Global.Animate;
using Global.Dialogue;
using Global.Mods;
using Global.Visible;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Object obj;

    private void Start () {
        GameObject topLayer = InputUtility.TopLayer;


    }

    private void OnValidate () {
        Debug.Log (obj);

    }

    private void Reset () {

    }


    private void LateUpdate () {
        // TMP_Text tmp = GameObject.FindObjectOfType<TMP_Text> ();
        // tmp.ForceMeshUpdate ();
        // Debug.Log (tmp.textInfo.linkCount);
        // // Debug.Log (tmp.textInfo.textComponent.alpha = 0);
        // tmp.textInfo.characterInfo[1].color = Color.red;
        // // tmp.textInfo.linkInfo[0].textComponent.alpha = 0;
        // Debug.Log (tmp.textInfo.characterCount);
        // Debug.Log (tmp.text);
    }
}