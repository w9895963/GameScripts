using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class AC_DebugAction : MonoBehaviour {


    public void Log (string sr) {
        Debug.Log (sr);
    }
    public void Log (GameObject sr) {
        Debug.Log (sr, sr);
    }

    public static void CreateSign (Vector2 position, float z = 0, float exitstTime = 1) {

        GameObject obj = Resources.Load ("DebugFile/sign", typeof (GameObject)) as GameObject;

        GameObject oj = GameObject.Instantiate (obj);
        oj.transform.position = position;
        oj.transform.rotation = Quaternion.Euler (0, 0, z);
        oj.GetComponent<autoDestroy> ().time = exitstTime;

    }


}