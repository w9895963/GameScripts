using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using Global.Skill;
using Global.Visible;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Global.Visible.VisibleUtility;

public class Test : MonoBehaviour {
    public Material material;
    public Vector2 v2;

    private void Start () {



    }

    private void OnValidate () {

    }




    private void Update () {
        material.SetVector ("Vector2", v2);
    }
}