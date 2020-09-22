using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public static class Layer {
        public static NormalLayer tempLayer = new NormalLayer ("Temp");

        public class NormalLayer {
            private string name;
            public NormalLayer (string name) {
                this.name = name;
            }
            public string Name {
                get => name;
            }
            public int Index {
                get => LayerMask.NameToLayer (name);
            }
            public int Mask {
                get => LayerMask.GetMask (name);
            }
        }
    }

    [System.Serializable] public class Curve {
        public float inputMax = 1;
        public float inputMin = 0;
        public float outputMax = 1;
        public float outputMin = 0;
        public AnimationCurve curve = ZeroOneCurve;
        public static AnimationCurve Default = ZeroOneCurve;


        public static AnimationCurve ZeroOneCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 0, 0, 0, 0, 0), new Keyframe (1, 1, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve OneZeroCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 1, 0, 0, 0, 0), new Keyframe (1, 0, 0, 0, 0, 0));
            }
        }
        public static AnimationCurve OneOneCurve {
            get {
                return new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 1));
            }
        }


        public float Evaluate (float index) {
            float i = (index - inputMin) / (inputMax - inputMin);
            return curve.Evaluate (i) * (outputMax - outputMin) + outputMin;
        }
        public static float Evaluate (float index, float inputMin, float inputMax,
            float outputMin, float outputMax, AnimationCurve curve = null
        ) {
            Curve cur = new Curve ();
            if (curve != null) cur.curve = curve;
            cur.inputMax = inputMax;
            cur.inputMin = inputMin;
            cur.outputMin = outputMin;
            cur.outputMax = outputMax;
            return cur.Evaluate (index);
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

}