using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
using Global.Mods;
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

    private void Start () {



    }

    private void OnValidate () {
        List<System.Reflection.FieldInfo> list = new List<System.Reflection.FieldInfo> ();




    }

    private void Reset () {
        var curr = Find.PlayerComp.setting;
        var lists1 = ModUtility.GetMembersFromObj (curr, typeof (Texture2D));
        Debug.Log (lists1.Count);

    }


    private void Update () { }
}