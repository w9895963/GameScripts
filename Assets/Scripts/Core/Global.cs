using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.U2D;


namespace Global {


  


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
            GameObject temp = Global.Find.TempObject;
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
            if (position != comp.gameObject.GetPosition2d ()) {
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
            GameObject temp = Global.Find.TempObject;

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




    public class TargetForce {
        public Profile vars;
        [System.Serializable] public class Profile {

            public Basic basic = new Basic ();
            [System.Serializable] public class Basic {
                public Vector2Ref target = new Vector2Ref ();
                public floatRef force = new floatRef (60);
            }
            public Optional optional = new Optional ();
            [System.Serializable] public class Optional {
                public bool ignoreMass = true;


                public bool enablePids = true;
                public PIDSetting pid = new PIDSetting ();
                [System.Serializable] public class PIDSetting : PIDv2.Basic { }

                public bool enableVelosityControl = false;
                public VelosityControl velosityControl = new VelosityControl ();
                [System.Serializable] public class VelosityControl {
                    public floatRef maxSpeed = new floatRef (6);
                    public float minSpeed = 0.1f;
                    public float slowDownDistance = 1;
                    public AnimationCurve slowDownCurve = Curve.ZeroOne;
                }


                public bool enableSingleDimension = false;
                public SingleDimension singleDimension = new SingleDimension ();
                [System.Serializable] public class SingleDimension {
                    public Vector2 dimensiion;
                }
            }
        }
        private GameObject gameObject;
        private PIDv2 pid2 = new PIDv2 ();

        public TargetForce (Profile profile, GameObject gameObject) {
            this.vars = profile;
            this.gameObject = gameObject;

            pid2.basic = profile.optional.pid;
            pid2.optional.enableMax = true;
            pid2.optional.maximum = profile.basic.force;
        }



        public void ApplyForce () {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();
            Vector2 position = rigidbody.position;
            Vector2 velocity = rigidbody.velocity;

            float force = vars.basic.force.v;

            Vector2 forceAdd = Vector2.zero;


            Vector2 target = vars.basic.target.v;
            if (vars.optional.enableSingleDimension) {
                SingleDimensionProcess (position, ref target);
            }

            Vector2 targetV;
            if (vars.optional.enableVelosityControl) {
                Vector2 distVt = target - position;
                var s = vars.optional.velosityControl;
                float mag = s.slowDownCurve.Evaluate (distVt.magnitude, 0, s.slowDownDistance, s.minSpeed, s.maxSpeed.v);
                targetV = distVt.normalized * mag;
            } else {
                Vector2 distVt = target - position;
                targetV = distVt;
            }



            if (vars.optional.enablePids) {
                forceAdd += pid2.CalcOutput (targetV - velocity);
            } else {
                Vector2 dir = target - position;
                Vector2 dirVt = (targetV - velocity);
                float scaler = dirVt.magnitude.ClampMax (1);
                forceAdd += dirVt.normalized * force * scaler;
            }

            if (vars.optional.enableSingleDimension) {
                forceAdd = forceAdd.Project (vars.optional.singleDimension.dimensiion);
            }


            if (vars.optional.ignoreMass) {
                forceAdd *= rigidbody.mass;
            }

            rigidbody.AddForce (forceAdd);
        }

        private void SingleDimensionProcess (Vector2 position, ref Vector2 target) {
            var s = vars.optional.singleDimension;
            Vector2 distVt = target - position;
            target = distVt.Project (s.dimensiion) + position;
        }

    }
    public class PIDv2 {
        public Basic basic = new Basic ();
        [System.Serializable] public class Basic {
            public float deltaRate = 0.3f;
            public float changedRate = 45f;
        }
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {
            public bool enableMax = false;
            public floatRef maximum = new floatRef (60);
        }
        private bool initial = false;
        private Vector2 integrate = default;
        private Vector2 lastError;


        public Vector2 CalcOutput (Vector2 error) {
            var s = basic;
            if (!initial) {
                lastError = error;
                initial = true;
            }
            Vector2 output;
            Vector2 delta = error - lastError;
            Vector2 wantDel = -error * s.deltaRate;
            Vector2 valueAdd = (wantDel - delta) * s.changedRate;
            integrate += -valueAdd;
            if (optional.enableMax) integrate = integrate.ClampMax (optional.maximum.v);

            lastError = error;
            output = integrate;

            float index = Time.time * 20;

            return output;
        }
    }

    /*  public class PIDv2 {
         public Basic basic = new Basic ();
         [System.Serializable] public class Basic {
             public float deltaRate = 0.3f;
             public float changedRate = 45f;
         }
         public Optional optional = new Optional ();
         [System.Serializable] public class Optional {
             public bool enableMax = false;
             public floatRef maximum = new floatRef (60);
         }
         private bool initial = false;
         private Vector2 integrate = default;
         private Vector2 lastError;


         public Vector2 CalcOutput (Vector2 error) {
             var s = basic;
             if (!initial) {
                 lastError = error;
                 initial = true;
             }
             Vector2 output;
             Vector2 delta = error - lastError;
             Vector2 wantDel = -error * s.deltaRate;
             Vector2 valueAdd = (wantDel - delta) * s.changedRate;
             integrate += -valueAdd;
             if (optional.enableMax) integrate = integrate.ClampMax (optional.maximum.v);

             lastError = error;
             output = integrate;

             float index = Time.time * 20;

             return output;
         }
     } */



    #region    //*Reference Value


    [System.Serializable] public class floatRef {
        public float v;
        public bool enabled = false;

        public floatRef (float value = 0) {
            this.v = value;
        }
    }

    [System.Serializable] public class boolRef {
        public bool v;

        public boolRef (bool value = false) {
            this.v = value;
        }
    }

    [System.Serializable] public class Vector2Ref {
        public Vector2 v;

        public Vector2Ref (Vector2 value = default) {
            this.v = value;
        }
    }
    #endregion
}