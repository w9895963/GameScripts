using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.Funtion;
using UnityEngine;
using UnityEngine.U2D;

public class Cz_PathMaker : MonoBehaviour {
    public int curr = 0;
    public EdgeCollider2D edge;
    void Start () {


    }

    void Update () {
        int count = edge.pointCount;
        if (0 < count) {
            //  edge.points[curr];
            //  GetComponent<Rigidbody2D>().

        }

    }


}