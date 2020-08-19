using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod {
    public static float GetMax (this AnimationCurve curve) {
        List<Keyframe> keyframes = new List<Keyframe> (curve.keys);
        keyframes.Sort ((a, b) => a.value > b.value? - 1 : 1);
        return keyframes[0].value;
    }

    public static float Evaluate (this AnimationCurve curve, float index, float indexMin, float indexMax,
        float valueMin, float valueMax) {
        float indexNew = (index - indexMin) / (indexMax - indexMin);

        return curve.Evaluate (indexNew) * (valueMax - valueMin) + valueMin;
    }
}