using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class Test : MonoBehaviour {
    public GameObject obj;
    void Start () {

    }

    void Update () {


        RaycastHit2D raycastHit2D = Physics2D.Raycast (
            obj.Get2dPosition (),
            Vector2.up,
            Mathf.Infinity,
            LayerMask.GetMask (Layer.tempLayer.Name)
        );
        Fn._.DrawPoint (raycastHit2D.point);
        // Fn._.DrawPoint (re[1].point);
        // Fn._.DrawLine (re[0].point, re[0].normal.normalized * re[0].distance);
    }
}