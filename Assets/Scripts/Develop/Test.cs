using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
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

        var curr = Find.Player.GetComponent<PlayerMnager> ().setting;
        List<System.Reflection.FieldInfo> currlist = curr.GetType ().GetFields ().ToList ();


        while (currlist.Count > 0) {
            var fieldInfos = currlist;
            var textures = fieldInfos.Where ((x) => x.FieldType == typeof (Texture2D)).ToList ();
            list.AddRange (textures);
            var classes = fieldInfos.Where ((x) => x.FieldType.IsClass).ToList ();
            currlist = classes.SelectMany ((x) => x.FieldType.GetFields ()).ToList ();
        }
        // Debug.Log (list.Count);

        var lists = Find.CursorComp.setting.GetType ().GetFields ();
        Debug.Log (lists[3].FieldType.IsList ());
        var obj = (IEnumerable) lists[3].GetValue (Find.CursorComp.setting);
        Debug.Log (obj);
        foreach (var item in obj) {
            Debug.Log (item);
        }

    }

    private void Reset () {

    }


    private void Update () { }
}