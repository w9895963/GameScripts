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




}