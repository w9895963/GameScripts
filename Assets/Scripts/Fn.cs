using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Fn {

//this
    public static void DrawCross (Vector2 position, float l = 0.2f) {

        Vector2 p = position;
        Vector2 p2 = position;
        Vector2 p3 = position;
        Vector2 p4 = position;
        p.x -= l;
        p2.x += l;
        p3.y -= l;
        p4.y += l;
        Debug.DrawLine (p, p2, Color.red, 1f);
        Debug.DrawLine (p3, p4, Color.red, 1f);
    }

    public static void DrawVector (Vector2 position, Vector2 vector) {
        Vector2 p2 = position + vector;

        Debug.DrawLine (position, p2, Color.red, 1f);
        DrawCross (p2);
    }


    public static Vector2 RotateClock (Vector2 vector, float angle) {
        return Quaternion.AngleAxis (angle, Vector3.back) * vector;
    }


    public static GameObject[] ArrayAddUniq (GameObject[] list, GameObject obj) {
        List<GameObject> l = new List<GameObject> (list);
        if (!l.Contains (obj)) l.Add (obj);
        return l.ToArray ();
    }
    public static GameObject[] ArrayRemove (GameObject[] list, GameObject obj) {
        List<GameObject> l = new List<GameObject> (list);
        l.Remove (obj);
        return l.ToArray ();
    }
}