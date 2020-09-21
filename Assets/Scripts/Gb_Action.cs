using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gb_Action : MonoBehaviour {


    public void _SetHelperCameraEnable (bool enable) {
        Fn._.FindGlobalObject (Global.GlobalObject.IndicatorCamera).GetComponent<Camera> ().enabled = enable;
    }

    public void _Gravity_Reverse () {
        FindObjectOfType<M_PlayerManager> ().ReverseGravity ();
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