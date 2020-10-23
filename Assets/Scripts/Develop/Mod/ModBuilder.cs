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

public class ModBuilder : MonoBehaviour {
    public Input input = new Input ();
    [System.Serializable] public class Input {
        public InputAction hierarchy;
        public InputAction inspector;
    }
    public ModProfile modProfile = new ModProfile ();
    [System.Serializable] public class ModProfile {
        public string modFolderName = "DefautMod";
        public string modName = "DefautMod";
        public int loadOrder = 0;
    }
    public GameObject modEditor;
    public Mod Mod {
        get {
            var mod = ModUtility.FindMod (modProfile.modFolderName);
            if (mod == null) {
                mod = ModUtility.CreateMod (modProfile.modFolderName, modProfile.modName, modProfile.loadOrder);
            }
            return mod;
        }
    }
    private GameObject holderObj;
    private GameObject hirObj;

    private void Awake () {
        input.hierarchy.performed += CallHierarchy;
        input.inspector.performed += CallInspector;
    }

    private void CallHierarchy (InputAction.CallbackContext _) {
        if (hirObj == null) {
            hirObj = GetComponentInChildren<RuntimeHierarchy> ().gameObject;
        }

        hirObj.SetActive (!hirObj.activeSelf);
    }

    private void CallInspector (InputAction.CallbackContext _) {
        RuntimeInspector runtimeInspector = GetComponentInChildren<RuntimeInspector> ();
        if (runtimeInspector != null) {
            Mod.WriteAllModData ();

            holderObj.Destroy ();
        } else {
            holderObj = Instantiate (modEditor);
            holderObj.SetParent (gameObject);


            Mod.LoadAllImage ();
        }
    }


    private void OnEnable () {
        input.hierarchy.Enable ();
        input.inspector.Enable ();
    }
    private void Start () { }

    private void OnDisable () {
        input.hierarchy.Disable ();
        input.inspector.Disable ();
    }




}