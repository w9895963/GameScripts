using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gb_EventAction : MonoBehaviour {


    public void _SetHelperCameraEnable (bool enable) {
        GlobalObject.Get (Global.GlobalObject.Type.IndicatorCamera).GetComponent<Camera> ().enabled = enable;
    }

    public void _Gravity_Reverse () {
        MainCharacter.ReverseGravity ();
    }


    public void _Scence_Reload () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
    public void _Scence_Load (string name) {
        SceneManager.LoadScene (name);
    }



    public void _Debug_Log (string sr) {
        Debug.Log (sr);
    }



}