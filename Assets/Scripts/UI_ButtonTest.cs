using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class UI_ButtonTest : MonoBehaviour {

    public M_Gravity importGravity;
    public int count;
    public float time;


    private void Update () {
        if (Time.time - time > 0.2f & count > 0) {
            Vector2 dir = Fn.RotateClock (importGravity.GetGravity (), count * 90);
            importGravity.SetGravityDirection (dir);
            count = 0;
        }

    }


    public void click () {
        count++;
        time = Time.time;
    }
}