using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.U2D;



public static class Extension_Fn {


    public static GameObject DrawLine (this Function fn, Vector2 start, Vector2 end, float time = 0.03f, float size = 0.4f) {
        if ((start - end).magnitude > 0.1f) {
            GameObject line = Resources.Load ("DebugFile/DotLine", typeof (GameObject)) as GameObject;
            GameObject lineNew = GameObject.Instantiate (line);
            lineNew.transform.parent = GlobalObject.TempObject.transform;
            if (time >= 0) lineNew.Ex_AutoDestroy (time);
            lineNew.transform.position = start;

            SpriteShapeController shape = lineNew.GetComponent<SpriteShapeController> ();
            Spline spline = shape.spline;

            spline.SetPosition (0, lineNew.transform.InverseTransformPoint (start));
            spline.SetHeight (0, size);
            spline.SetPosition (1, lineNew.transform.InverseTransformPoint (end));
            spline.SetHeight (1, size);
            return lineNew;
        } else {
            return null;
        }
    }
    public static GameObject DrawPoint (this Function fn, Vector2 position,
        float sizePixel = 6f, float stayTime = 0.03f, Color color = default) {
        /////////////////////////////////////////////////////
        GameObject point = Resources.Load ("DebugFile/IndicatePoint", typeof (GameObject)) as GameObject;
        point = GameObject.Instantiate (point, (Vector3) position, Quaternion.Euler (0, 0, 0));
        point.transform.parent = GlobalObject.TempObject.transform;
        if (stayTime > 0) point.Ex_AutoDestroy (stayTime);
        float heightUnit = Camera.main.orthographicSize * 2;
        float pixelUnit = Screen.height / heightUnit;
        float scale = sizePixel / pixelUnit;
        point.transform.localScale = new Vector3 (scale, scale, 1);
        if (color != default) point.GetComponent<SpriteRenderer> ().color = color;
        return point;
    }

    public static GameObject WaitToCall (this Function fn, float time, UnityAction action) {
        Timer.TimerControler timerControler = Timer.WaitToCall (time, action, creator : fn.callBy);
        return Timer.TimerManager.gameObject;
    }



}

namespace Global {


    public class Function {
        public Object callBy;
        public Function (Object callBy) {
            this.callBy = callBy;
        }
        public static Function CallFn (Object callBy = null) {
            return new Function (callBy);
        }



        public static List<T> FindAllInterfaces<T> () where T : class {
            MonoBehaviour[] holders = GameObject.FindObjectsOfType<MonoBehaviour> ();
            return holders.OfType<T> ().ToList ();
        }
        public static GameObject FindObjectWithInterfaces<T> () where T : class {
            MonoBehaviour[] holders = GameObject.FindObjectsOfType<MonoBehaviour> ();
            IEnumerable<T> enumerable = holders.OfType<T> ();
            MonoBehaviour monoBehaviour = enumerable.First () as MonoBehaviour;
            return monoBehaviour.gameObject;
        }

    }

}