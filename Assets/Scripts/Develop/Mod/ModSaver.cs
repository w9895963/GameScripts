using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using Global.Mods;
using RuntimeInspectorNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModSaver : MonoBehaviour {
    public string modFolderName = "DefautMod";
    public string modName = "DefautMod";
    private bool saveToDisk = false;
    public bool SaveToDisk {
        set {
            saveToDisk = value;
            if (value == true)
                SaveFiles ();
        }
        get => saveToDisk;
    }
    private Mod mod;



    private void Awake () {
        mod = ModUtility.FindMod (modFolderName);
        if (mod != null) {
            mod = ModUtility.CreateMod (modFolderName, modName);
        }
    }

    private void Start () {
        mod.LoadAllImage ();

    }

    private void OnApplicationQuit () {
        mod.WriteAllModData ();
    }


    public void SaveFiles () {
        mod.WriteAllModData ();
    }



}