﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class I_Throw : IC_Base {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Rigidbody2D rigidBody;
        public float force = 90;
        public Vector2 apllyPoint = new Vector2 (0.1f, 0.1f);
        public string dataname;
    }



     void OnEnable () {
        data.actionIndex = 0;
        Vector2 manP = Gb.MainCharactor.transform.position;
        setting.rigidBody.position = manP;
        Vector2 position = Pointer.current.position.ReadValue ().ScreenToWold ();
        var v = data.shareData.Get (setting.dataname, ShareDataType.Vector2);
        if (v != null) position = v.vector2Data;

        Vector2 force = (position - manP).normalized * setting.force;


        Vector2 apllyPoint = setting.apllyPoint;
        if (apllyPoint != default) {
            setting.rigidBody.AddForceAtPosition (force, apllyPoint, ForceMode2D.Impulse);
        } else {
            setting.rigidBody.AddForce (force, ForceMode2D.Impulse);
        }

        enabled = false;

    }

     void OnDisable () {

    }
}