using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {


    }




    private void FixedUpdate () {
        GetComponent<PhysicGravity>().ResetGravity();

    }
    public Vector2 Gravity {
        get =>
            throw new System.NotImplementedException ();
        set =>
            throw new System.NotImplementedException ();
    }

    public void ResetGravity () {
        throw new System.NotImplementedException ();
    }
}