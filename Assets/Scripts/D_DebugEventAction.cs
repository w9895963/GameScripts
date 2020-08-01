using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class D_DebugEventAction : MonoBehaviour {


    public void Log (string sr) {
        Debug.Log (sr);
    }
    public void Log (GameObject sr) {
        Debug.Log (sr, sr);
    }

    // public static void CreateSign (Vector2 position) {
    //     GameObject obj = Resources.Load ("Assets/DebugActionFile/sign.prefab") as GameObject;
    //     GameObject.Instantiate (obj).transform.position = position;

    // }
    public void Test () {

    }


}