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
        private const string textureName = "PointData";
        private const int pix = 8;

        public static void AddKey (int curveIndex, float index, float value,
            Color color = default, Vector2 position = default, float angleLimit = 1
        ) {
            GameObject temp = GlobalObject.TempObject;
            Vector2 point = new Vector2 (index, value);
            var list = temp.GetComponentsInChildren<LineRenderer> ().ToList ();
            list = list.FindAll ((x) => x.name == objectName);
            if (list.Count <= curveIndex) {
                for (int i = list.Count; i <= curveIndex; i++) {
                    Create ();
                }
                list = temp.GetComponentsInChildren<LineRenderer> ().ToList ();
                list = list.FindAll ((x) => x.name == objectName);
            }
            LineRenderer comp = list[curveIndex];
            if (color != default) {
                comp.endColor = color;
                comp.startColor = color;
            }
            if (position != comp.gameObject.Get2dPosition ()) {
                comp.gameObject.SetPosition (position);
            }


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
            int indexP;
            if (angleTest) {
                comp.positionCount = count + 1;
                indexP = count;
            } else {
                indexP = count - 1;
            }
            comp.SetPosition (indexP, point);
            //*store point data to texture
            PointToColorData (indexP, point, comp);


        }
        public static void PointToColorData (int pointI, Vector2 pointP, LineRenderer comp) {
            Texture2D texture = comp.materials[0].GetTexture (textureName) as Texture2D;
            if (texture == null) {
                texture = new Texture2D (16, 16, TextureFormat.RGFloat, false);
                comp.materials[0].SetTexture (textureName, texture);
            }
            MaterialFn.StoreDataToTexture (texture, pointI, pointP, TextureFormat.RGFloat, 16);

        }

        public static void Create (Color color = default, Vector2 position = default) {
            GameObject temp = GlobalObject.TempObject;

            GameObject obj = GameObject.Instantiate (Resources.Load (lineTemplate, typeof (GameObject)) as GameObject);
            obj.name = objectName;
            obj.transform.parent = temp.transform;
            obj.transform.position = position;
            LineRenderer line = obj.GetComponent<LineRenderer> ();
            if (color == default) {
                color = Color.white;
            }
            line.endColor = color;
            line.startColor = color;



        }

    }


    public static class MaterialFn {
        public static void StoreDataToTexture (Texture2D texture, int index, Vector4 data,
            TextureFormat formate, int pixelsUnit = 16
        ) {
            int width = texture.width;
            int height = texture.height;
            int edge = (int) Mathf.Ceil (Mathf.Sqrt (index + 1));
            int needEdge = (int) Mathf.Ceil (edge / (float) pixelsUnit) * pixelsUnit;
            if (texture.format != formate | width != needEdge) {
                Color[] colors = texture.GetPixels ();
                texture.Resize (needEdge, needEdge, formate, false);
                texture.SetPixels (0, 0, width, width, colors);
                texture.filterMode = FilterMode.Point;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply ();
            }

            int row = (int) Mathf.Floor (Mathf.Sqrt (index));
            int subIndex = index - (int) Mathf.Pow (row, 2);
            Vector2 coor;
            if (subIndex <= row) {
                coor = new Vector2 (subIndex, row);
            } else {
                coor = new Vector2 (row, 2 * row - subIndex);
            }
            texture.SetPixel ((int) coor.x, (int) coor.y, new Color (data.x, data.y, data.z, data.w));
            texture.Apply ();
        }

    }




}


public static class Extension_Global {
    public static float Evaluate (this AnimationCurve curve, float index, float inputMin, float inputMax,
        float outputMin = 0, float outputMax = 1) {
        Curve cur = new Curve ();
        cur.curve = curve;
        cur.inputMax = inputMax;
        cur.inputMin = inputMin;
        cur.outputMin = outputMin;
        cur.outputMax = outputMax;
        return cur.Evaluate (index);
    }


}