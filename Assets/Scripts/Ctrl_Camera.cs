using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_Camera : MonoBehaviour {

    [System.Serializable] public class Follow {
        public bool enabled = false;
        public C1_Follow.Setting setting = new C1_Follow.Setting ();

    }
    public Follow follow = new Follow ();
    [SerializeField, ReadOnly] private List<Object> temps = new List<Object> ();

    //*---------------
    private void OnEnable () {
        if (follow.enabled) {
            temps.Add (
                gameObject._ ().Follow (follow.setting)
            );


        }
    }

    private void OnDisable () {
        temps.Destroy ();

    }


    //* Public Method
    public static void FollowCharactor () {

    }



}

namespace Globle {
    public static class CameraCtrl {

    }
}