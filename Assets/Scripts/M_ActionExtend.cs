using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ActionExtend : MonoBehaviour {



    public void _Animation_Set_True (string name) {
        GetComponent<Animator> ()?.SetBool (name, true);
    }
    public void _Animation_Set_False (string name) {
        GetComponent<Animator> ()?.SetBool (name, false);
    }
}