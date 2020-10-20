using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using Global.Skill;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public float time;
    public GameObject target;

    private void Start () {
        new GravityReverse (target, time);

    }

    private void OnValidate () {
        GetComponent<InputField> ();

    }




    private void Update () { }
}