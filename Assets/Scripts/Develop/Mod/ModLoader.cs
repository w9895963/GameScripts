using System.Collections;
using System.Collections.Generic;
using Global.Mods;
using UnityEngine;
public class ModLoader : MonoBehaviour {
    private void Awake () {
        ModFunc.LoadAllModData ();
    }
}