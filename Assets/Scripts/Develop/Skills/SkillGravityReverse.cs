using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class SkillGravityReverse : MonoBehaviour {
    public float lastTime = 1;


    private void OnEnable () {
        GetComponent<IGravity> ().Gravity *= -1;
    }
    private void OnDisable () {
        GetComponent<IGravity> ().ResetGravity ();
    }

    private void Update () {
        lastTime -= Time.deltaTime;
        if (lastTime <= 0) {
            Destroy (this);
        }
    }

}