using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using Global.Skill;
using Global.Visible;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Global.Visible.VisibleUtility;

public class Test : MonoBehaviour {
    public float time;
    public GameObject target;

    private void Start () {
        GravityReverse gravityReverse = new GravityReverse ();
        gravityReverse.lastTime = 3;
      

        AddCountDown (target, time, target.GetPositionBottomLeft () - new Vector2 (-0.05f, 0.25f));

    }

    private void OnValidate () {
        GetComponent<InputField> ();

    }




    private void Update () { }
}