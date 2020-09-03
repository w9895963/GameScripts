using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_InPack : IC_Base {
    public GameObject targetObject;



     void OnEnable () {
        data.actionIndex = 0;

        Gb.Backpack.PutinStorage (this);
    }
     void OnDisable () {


    }

}