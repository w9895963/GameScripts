using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.U2D;

namespace Global {
  

    [System.Serializable] public class Curve {
        public float inputMax = 1;
        public float inputMin = 0;
        public float outputMax = 1;
        public float outputMin = 0;
        public AnimationCurve curve = ZeroOne;
        public static AnimationCurve Default = ZeroOne;


        public static AnimationCurve ZeroOne {
            get {
                return new AnimationCurve (new Keyframe (0, 0, 0, 0, 0, 0), new Keyframe (1, 1, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve ZeroOneFastOut01 {
            get {
                return new AnimationCurve (new Keyframe (0, 0, 0, 1.5f, 0, 0.4f), new Keyframe (1, 1, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve ZeroOneSmooth03 {
            get {
                return new AnimationCurve (new Keyframe (0, 0, 0, 0, 0, 0.3f), new Keyframe (1, 1, 0, 0, 0.3f, 0));
            }
        }
        public static AnimationCurve OneZero {
            get {
                return new AnimationCurve (new Keyframe (0, 1, 0, 0, 0, 0), new Keyframe (1, 0, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve OneOneCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 1));
            }
        }
        public static AnimationCurve ZeroZero {
            get {
                return new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 0));
            }
        }


        public float Evaluate (float index) {
            float i = (index - inputMin) / (inputMax - inputMin);
            return curve.Evaluate (i) * (outputMax - outputMin) + outputMin;
        }
        public static float Evaluate (float index, float inputMin, float inputMax,
            float outputMin, float outputMax, AnimationCurve curve = null
        ) {
            if (curve != null) {
                Curve cur = new Curve ();
                cur.curve = curve;
                cur.inputMax = inputMax;
                cur.inputMin = inputMin;
                cur.outputMin = outputMin;
                cur.outputMax = outputMax;
                return cur.Evaluate (index);
            } else {
                return (index.Clamp (inputMin, inputMax) - inputMin) / (inputMax - inputMin)
                    * (outputMax - outputMin) + outputMin;
            }
        }
    }

    [System.Serializable] public class Path {
        [System.Serializable] public class CurveSetting {
            public Vector2 start = Vector2.zero;
            public Vector2 end = Vector2.right;
            public int sampleNum = 8;
            public float maxAngle = 60;
            public AnimationCurve curve = Curve.ZeroOne;
            public AnimationCurve curveVertical = Curve.ZeroZero;
            public List<Vector3> plist = new List<Vector3> ();
        }

        public List<Vector2> GenPathFromSetting (CurveSetting setting) {
            CurveSetting s = setting;
            s.plist = new List<Vector3> ();
            for (int i = 0; i <= s.sampleNum; i++) {
                float rate = 1f / s.sampleNum * i;
                Vector2 p = GetPosition (rate, s);
                s.plist.Add (p.ToVector3 (rate));
            }

            subdivision (s);

            var dist = (s.end - s.start).magnitude;
            Quaternion qu = Quaternion.FromToRotation (Vector3.right, (s.end - s.start).ToVector3 ());
            return s.plist.Select ((x) => (qu * new Vector2 (x.x, x.y)).ToVector2 () * dist + s.start).ToList ();

        }

        private Vector2 GetPosition (float rate, CurveSetting curveSetting) {
            CurveSetting s = curveSetting;
            float x = s.curve.Evaluate (rate);
            float y = s.curveVertical.Evaluate (rate);
            Vector2 p = new Vector2 (x, y);
            return p;
        }
        private void subdivision (CurveSetting curveSetting) {
            bool sub = false;
            CurveSetting s = curveSetting;
            for (int i = 1; i < s.plist.Count - 1; i++) {
                float angle = Vector2.Angle ((s.plist[i] - s.plist[i - 1]).ToVector2 (),
                    (s.plist[i + 1] - s.plist[i]).ToVector2 ());
                if (angle > s.maxAngle) {
                    float rate;
                    Vector2 p;
                    rate = (s.plist[i].z + s.plist[i - 1].z) / 2f;
                    p = GetPosition (rate, s);
                    s.plist.Insert (i, p.ToVector3 (rate));
                    rate = (s.plist[i + 2].z + s.plist[i + 1].z) / 2f;
                    p = GetPosition (rate, s);
                    s.plist.Insert (i + 2, p.ToVector3 (rate));
                    sub = true;
                    break;
                }

            }
            if (sub) {
                subdivision (s);
            }
        }

    }

    [System.Serializable] public class RefVector2 {
        public Vector2 value;

        public RefVector2 (Vector2 vector = default) { value = vector; }
    }

    [System.Serializable] public class RefFloat {
        public float value;
        public RefFloat (float value = 0) { this.value = value; }
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
    public static class VisibalCurve {
        private const string objectName = "VisibalCurve";
        private const string lineTemplate = "DebugFile/LineTemplate";

        public static void AddKey (int curveIndex, float index, float value, float angleLimit = 2) {
            GameObject temp = GlobalObject.TempObject;
            Vector2 point = new Vector2 (index * 10, value * 10);
            var list = temp.GetComponentsInChildren<LineRenderer> ().ToList ();
            list = list.FindAll ((x) => x.name == objectName);
            LineRenderer comp = list[curveIndex];

            int count = comp.positionCount;
            bool angleTest = true;
            if (count >= 3) {
                Vector2 p1 = comp.GetPosition (count - 1);
                Vector2 p0 = comp.GetPosition (count - 2);
                var p2 = point;
                float angle = Vector2.Angle (p1 - p0, p2 - p1);
                if (angle < angleLimit) {
                    angleTest = false;
                }
            }
            if (angleTest) {
                comp.positionCount = count + 1;
                comp.SetPosition (count, point);
            } else {
                comp.SetPosition (count - 1, point);
            }
        }

        public static void Create (Color color = new Color (), Vector2 position = default) {
            GameObject temp = GlobalObject.TempObject;

            GameObject obj = GameObject.Instantiate (Resources.Load (lineTemplate, typeof (GameObject)) as GameObject);
            obj.name = objectName;
            obj.transform.parent = temp.transform;
            obj.transform.position = position;
            LineRenderer line = obj.GetComponent<LineRenderer> ();
            line.endColor = color;
            line.startColor = color;




        }

    }

}


public static class Extension_AnimationCurve {
    public static float Evaluate (this AnimationCurve curve, float index, float inputMin, float inputMax,
        float outputMin, float outputMax) {
        Curve cur = new Curve ();
        cur.curve = curve;
        cur.inputMax = inputMax;
        cur.inputMin = inputMin;
        cur.outputMin = outputMin;
        cur.outputMax = outputMax;
        return cur.Evaluate (index);
    }

}