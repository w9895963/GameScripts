using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using RuntimeInspectorNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModEditor : MonoBehaviour {
    public string modFolderName = "DefautMod";
    public string modName = "DefautMod";
    public Input input = new Input ();
    [System.Serializable] public class Input {
        public InputAction hierarchy;
        public InputAction inspector;
        public InputAction save;
        public InputAction load;
    }
    private bool save = false;
    public bool SaveToDisk {
        set {
            save = value;
            SaveFiles ();
        }
        get => save;
    }
    private ModUtility.Mod mod;
    private RuntimeHierarchy hierarchy;
    private RuntimeInspector inspector;



    private void Awake () {
        hierarchy = transform.parent.GetComponentInChildren<RuntimeHierarchy> ();
        inspector = transform.parent.GetComponentInChildren<RuntimeInspector> ();
        input.hierarchy.performed += (d) => {
            GameObject hrObj = hierarchy.gameObject;
            if (!hrObj.activeSelf) {
                hrObj.SetActive (true);
            } else {
                hrObj.SetActive (false);
            }
        };
        input.inspector.performed += (d) => {
            GameObject inObj = inspector.gameObject;
            if (!inObj.activeSelf) {
                inObj.SetActive (true);
            } else {
                inObj.SetActive (false);
            }
        };
        input.save.performed += (d) => {
            SaveFiles ();
        };
        input.load.performed += (d) => {
            ModUtility.LoadAllModData ();
        };


        mod = ModUtility.GetMod (modFolderName);
        if (mod != null) {
            mod = ModUtility.CreateMod (modFolderName, modName);
        }
    }

    private void OnEnable () {
        input.hierarchy.Enable ();
        input.inspector.Enable ();
        input.save.Enable ();
        input.load.Enable ();
    }
    private void Start () {
        mod.LoadAllImage ();
        CreateFakeScence ();

        hierarchy.gameObject.SetActive (false);
        inspector.gameObject.SetActive (false);
    }

    private void OnDisable () {
        input.hierarchy.Disable ();
        input.inspector.Disable ();
        input.save.Disable ();
        input.load.Disable ();
    }

    private void CreateFakeScence () {
        RuntimeHierarchy hra = transform.parent.GetComponentInChildren<RuntimeHierarchy> ();
        hra.CreatePseudoScene ("Mod");
        hra.AddToPseudoScene ("Mod", GameObject.FindObjectOfType<M_Cursor> ().transform);
        hra.AddToPseudoScene ("Mod", GameObject.FindObjectOfType<ModEditor> ().transform);
    }



    private void SaveFiles () {
        mod.WriteAllModData ();
    }



}