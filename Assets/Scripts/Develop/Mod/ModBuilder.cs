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
        public InputAction save;
        public InputAction load;
    }
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
            GetComponent<ModSaver> ().SaveFiles ();
        };
        input.load.performed += (d) => {
            ModUtility.LoadAllModData ();
        };


    }

    private void OnEnable () {
        input.hierarchy.Enable ();
        input.inspector.Enable ();
        input.save.Enable ();
        input.load.Enable ();
    }
    private void Start () {
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
        hra.AddToPseudoScene ("Mod", GameObject.FindObjectOfType<ModBuilder> ().transform);
    }




}