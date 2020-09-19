using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_InPack : IC_Base {
    public GameObject targetObject;



    void OnEnable () {

        // Gb.Backpack.PutinStorage (this);
        Gb._.backpack.obj.PutinStorage (this);
    }
    void OnDisable () {


    }

}