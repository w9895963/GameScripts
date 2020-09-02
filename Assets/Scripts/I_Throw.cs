using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class I_Throw : IC_Base {
    public Rigidbody2D rigidBody;
    public float force = 90;
    public Vector2 apllyPoint = new Vector2 (0.1f, 0.1f);
    public string dataname;

    public override void OnEnable_ () {
        Vector2 manP = Gb.MainCharactor.transform.position;
        rigidBody.position = manP;
        Vector2 position = Pointer.current.position.ReadValue ().ScreenToWold ();
        position = data.shareData.Get (dataname, ShareDataType.Vector2).vector2Data;

        rigidBody.AddForce ((position - manP).normalized * force, ForceMode2D.Impulse);

    }

    public override void OnDisable_ () {

    }
}