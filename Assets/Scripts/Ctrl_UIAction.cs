using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ctrl_UIAction : MonoBehaviour {
    public void _ReverseGravity () {
        MainCharacter.ReverseGravity ();
    }

    public void _Scence_Load (string name) {
        SceneManager.LoadScene (name);
    }
    public void _Scence_Load (int index) {
        if (SceneManager.GetActiveScene ().buildIndex != index) {
            SceneManager.LoadScene (index);
        }
    }

    public void _SetHelperCameraEnable (bool enable) {
        Global.Find.IndicateCamera.GetComponent<Camera> ().enabled = enable;
    }

    public void _CUrsor_VisibleSystemCursor (bool enabled) => CursorCtrl.ShowSystemCursor (enabled);

}