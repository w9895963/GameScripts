using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_InPack : IC_Base {
    public GameObject targetObject;



    public override void EnableAction () {

        targetObject.Ex_Hide ();
        Gb.Backpack.PutinStorage (this);
    }
    public override void DisableAction () {

        targetObject.Ex_Show ();

    }

}