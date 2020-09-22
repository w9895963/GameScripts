﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.U2D;



public static class Extension_Fn {

    //*Event Trigger

    public static GameObject DrawLine (this Funtion fn, Vector2 start, Vector2 end, float time = 0.03f) {
        if ((start - end).magnitude > 0.1f) {
            GameObject line = Resources.Load ("DebugFile/DotLine", typeof (GameObject)) as GameObject;
            GameObject lineNew = GameObject.Instantiate (line);
            if (time >= 0) lineNew.Ex_AutoDestroy (time);
            lineNew.transform.position = start;

            SpriteShapeController shape = lineNew.GetComponent<SpriteShapeController> ();
            Spline spline = shape.spline;

            spline.SetPosition (0, lineNew.transform.InverseTransformPoint (start));
            spline.SetHeight (0, 0.2f);
            spline.SetPosition (1, lineNew.transform.InverseTransformPoint (end));
            spline.SetHeight (1, 0.2f);
            return lineNew;
        } else {
            return null;
        }
    }
    public static GameObject DrawPoint (this Funtion fn, Vector2 position,
        float sizePixel = 6f, float stayTime = 0.03f, Color color = default) {
        /////////////////////////////////////////////////////
        GameObject point = Resources.Load ("DebugFile/IndicatePoint", typeof (GameObject)) as GameObject;
        point = GameObject.Instantiate (point, (Vector3) position, Quaternion.Euler (0, 0, 0));
        if (stayTime > 0) point.Ex_AutoDestroy (stayTime);
        float heightUnit = Camera.main.orthographicSize * 2;
        float pixelUnit = Screen.height / heightUnit;
        float scale = sizePixel / pixelUnit;
        point.transform.localScale = new Vector3 (scale, scale, 1);
        if (color != default) point.GetComponent<SpriteRenderer> ().color = color;
        return point;
    }

    public static GameObject WaitToCall (this Funtion fn, float time, UnityAction action) {
        GameObject obj = new GameObject ("Timer");
        Fn_Timer timer = obj.AddComponent<Fn_Timer> ();
        timer.WaitToCall (time, action);
        return obj;
    }




}

namespace Global {


    public class Funtion {
        public Object callBy;
        public Funtion (Object callBy) {
            this.callBy = callBy;
        }
        public static Funtion Fn (Object callBy) {
            return new Funtion (callBy);
        }


    }

}