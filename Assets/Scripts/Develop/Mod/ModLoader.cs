using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
public class ModLoader : MonoBehaviour {
    private void Awake () {
        ModUtility.LoadAllModData ();
    }
}