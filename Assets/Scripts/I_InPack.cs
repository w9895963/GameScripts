using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_InPack : IC_Base {
    public GameObject targetObject;



    void OnEnable () {
        M_BackPack backpack = FindObjectOfType<M_BackPack> ();

        backpack.PutinStorage (this);
    }
    void OnDisable () {


    }

}