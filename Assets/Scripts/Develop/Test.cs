using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Test : MonoBehaviour {
    public RuntimeAnimatorController obj;

    private void Awake () { }

    private void OnValidate () {
        // Debug.Log (Directory.Exists ("D:/Downloads/Mouse-cursor-hand-pointer.svg"));
        // Debug.Log (File.Exists ("D:/Downloads/Mouse-cursor-hand-pointer.svg"));

        // ModUtility.SpriteData spriteData = new ModUtility.SpriteData (FileUtility.GetFullPath ("Mods/last.png"));
        // spriteData.WriteToDisk ();

    }


    private void Update () { }
}