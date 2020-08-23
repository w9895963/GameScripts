using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod {
    public static float GetMax (this AnimationCurve curve) {
        List<Keyframe> keyframes = new List<Keyframe> (curve.keys);
        keyframes.Sort ((a, b) => a.value > b.value? - 1 : 1);
        return keyframes[0].value;
    }

    public static float Evaluate (this AnimationCurve curve, float index,
        float indexMin, float indexMax,
        float valueMin, float valueMax) {


        float indexNew = (index - indexMin) / (indexMax - indexMin);
        return curve.Evaluate (indexNew) * (valueMax - valueMin) + valueMin;
    }


    public static T[] Add<T> (this T[] source, T newMember) {
        List<T> list = new List<T> (source);
        list.Add (newMember);
        return list.ToArray ();
    }

    public static Vector2 ProjectOnPlane (this Vector2 vector, Vector2 normal) {
        return (Vector2) Vector3.ProjectOnPlane (vector, normal);
    }
    public static Vector2 Project (this Vector2 vector, Vector2 direction) {
        return (Vector2) Vector3.Project (vector, direction);
    }
    public static Vector2 ScreenToWold (this Vector2 vector) {
        return Camera.main.ScreenToWorldPoint (vector);
    }

}