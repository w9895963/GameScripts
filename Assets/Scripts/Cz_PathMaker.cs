using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.Function;
using UnityEngine;
using UnityEngine.U2D;

public class Cz_PathMaker : MonoBehaviour {
    public int curr = 0;
    public EdgeCollider2D edge;
    void Start () {
        List<Vector3> lists = ToPointsList (edge.points);


    }

    void Update () {

    }

    private static List<Vector3> ToPointsList (Vector2[] points) {
        List<Vector3> list = new List<Vector3> ();
        int count = points.Length;
        if (count > 0) {
            list.Add (points[0]);

            for (int i = 1; i < count; i++) {
                float dist = (points[i] - points[i - 1]).magnitude + list[i - 1].z;
                Vector3 newP = new Vector3 (points[i].x, points[i].y, dist);
                list.Add (newP);
            }

        }
        return list;
    }
}