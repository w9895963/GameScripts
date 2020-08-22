using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public static class Fn {

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

    public static void DrawVector (Vector2 position, Vector2 vector, float time = 1f) {
        Vector2 p2 = position + vector;

        Debug.DrawLine (position, p2, Color.red, time);
        //Debug.DrawLine (position, p2, Color.red);
        DrawCross (p2);
    }

    public static GameObject Create (GameObject obj, Vector2 position, float rotate = 0) {
        return GameObject.Instantiate (obj, position, Quaternion.AngleAxis (rotate, Vector3.forward));
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


    public static void WaitToCall (float time, UnityAction call) {
        GameObject obj = new GameObject ();
        Fn_Timer timer = obj.AddComponent<Fn_Timer> ();
        timer.WaitToCall (time, call);
    }

    public static EventTrigger.Entry AddEventToTrigger (GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger eventTrigger = obj.GetComponent<EventTrigger> ();
        EventTrigger trigger = eventTrigger != null ? eventTrigger : obj.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener (action);
        trigger.triggers.Add (entry);
        return entry;
    }
    public static void RemoveEventToTrigger (GameObject obj, EventTriggerType type, EventTrigger.Entry entry) {
        EventTrigger eventTrigger = obj.GetComponent<EventTrigger> ();
        EventTrigger trigger = eventTrigger != null ? eventTrigger : obj.AddComponent<EventTrigger> ();

        trigger.triggers.Remove (entry);
    }

    public static void AddOneTimeListener (UnityEvent[] eventList, UnityAction atn) {
        UnityAction action = null;
        action = () => {
            atn ();
            foreach (var e in eventList)
                e.RemoveListener (action);
        };
        foreach (var e in eventList)
            e.AddListener (action);

    }
    public static void AddOneTimeListener (UnityEvent singleEvent, UnityAction atn) {
        UnityAction action = null;
        action = () => {
            atn ();
            singleEvent.RemoveListener (action);
        };
        singleEvent.AddListener (action);

    }
    public static M_OnArrive OnArrive (GameObject gameObject, Vector2 position,
        UnityAction callBack, Vector2 distanceDirection = default,
        float distance = 0.01f, bool autoDestroy = true) {


        M_OnArrive comp = gameObject.AddComponent<M_OnArrive> ();
        comp.SetPosition (position, distanceDirection, distance);
        comp.SetEvent (callBack, autoDestroy);

        return comp;

    }


    public class LayerRender {
        public Vector3 cameraPosition;
        public Vector3 spritePosition;
        public float cameraSize;
        public Vector2 resolution;
        public float pixelsPerUnit;
        public Vector3 scale;


        public LayerRender (SpriteRenderer refSpriteRender, Bounds clacBounds) {
            CalcDates (refSpriteRender, clacBounds);
        }

        private void CalcDates (SpriteRenderer refSpriteRender, Bounds calcBounds) {


            float pixelUnit = refSpriteRender.sprite.rect.height / refSpriteRender.bounds.size.y;
            Vector3 origin = refSpriteRender.bounds.min;

            Vector3 center = calcBounds.center;
            Vector3 max = calcBounds.max;
            Vector3 min = calcBounds.min;

            if (calcBounds != refSpriteRender.bounds) {
                max.x = Mathf.Ceil ((max.x - origin.x) * pixelUnit) / pixelUnit + origin.x;
                max.y = Mathf.Ceil ((max.y - origin.y) * pixelUnit) / pixelUnit + origin.y;
                min.x = Mathf.Floor ((min.x - origin.x) * pixelUnit) / pixelUnit + origin.x;
                min.y = Mathf.Floor ((min.y - origin.y) * pixelUnit) / pixelUnit + origin.y;

                center.x = (max.x - min.x) / 2 + min.x;
                center.y = (max.y - min.y) / 2 + min.y;
            }


            spritePosition = center;
            cameraPosition = center;
            cameraPosition.z -= 10;
            cameraSize = (max.y - min.y) / 2;
            resolution.x = (max.x - min.x) * pixelUnit;
            resolution.y = (max.y - min.y) * pixelUnit;
            pixelsPerUnit = refSpriteRender.sprite.pixelsPerUnit;
            scale = refSpriteRender.transform.lossyScale;
            scale.x = scale.y;


        }
        public Sprite RenderToSprite (int layerMasks, float resolutionScale = 1) {
            return RenderLayerToSprite (cameraPosition, cameraSize, resolution * resolutionScale,
                pixelsPerUnit * resolutionScale, layerMasks);
        }

        public static Sprite RenderLayerToSprite (Vector3 cameraPosition, float cameraSize, Vector2 resolution, float pixelsPerUnit, int layerMaks) {

            Vector3 center = cameraPosition;
            float camH = cameraSize;
            int w = Mathf.RoundToInt (resolution.x);
            int h = Mathf.RoundToInt (resolution.y);


            RenderTexture te = new RenderTexture (w, h, 24);
            Camera cam = new GameObject ("Cam", typeof (Camera)).GetComponent<Camera> ();

            cam.transform.position = new Vector3 (center.x, center.y, center.z);
            cam.orthographic = true;
            cam.orthographicSize = camH;
            cam.targetTexture = te;
            cam.cullingMask = layerMaks;
            cam.Render ();



            Rect r = new Rect (0, 0, w, h);
            Texture2D t = new Texture2D ((int) w, (int) h);
            t.filterMode = FilterMode.Point;
            RenderTexture.active = te;
            t.ReadPixels (r, 0, 0);
            t.Apply ();
            RenderTexture.active = null;

            Sprite s = Sprite.Create (t, r, new Vector2 (0.5f, 0.5f), pixelsPerUnit);

            Object.Destroy (cam.gameObject);
            Object.Destroy (te);

            return s;

        }

    }

    [System.Serializable]
    public class Curve {
        public float inputMax = 1;
        public float inputMin = 0;
        public float outputMax = 10;
        public float outputMin = 0;
        public AnimationCurve curve = ZeroOneCurve;

        public static AnimationCurve ZeroOneCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 0, 0, 0, 0, 0), new Keyframe (1, 1, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve OneOneCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 1, 0, 0, 0, 0), new Keyframe (1, 1, 0, 0, 0, 0));
            }
        }


        public float Evaluate (float index) {
            float i = (index - inputMin) / (inputMax - inputMin);
            // pr
            return curve.Evaluate (i) * (outputMax - outputMin) + outputMin;
        }
    }
}