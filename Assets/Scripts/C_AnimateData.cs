using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_AnimateData : MonoBehaviour {
    public float time;
    public AnimationCurve curve = Global.Curve.ZeroOneCurve;
    public UnityAction<float> onFloatAnimationEnd;
    public UnityAction<float> onFloatAnimation;
    public UnityAction<Vector2> onVector2AnimationEnd;
    public UnityAction<Vector2> onVector2Animation;
    public Data resultData = new Data ();
    [ReadOnly] public float timebegin;
    [SerializeField, ReadOnly] private Object createBy = null;
    public Test test = new Test ();
    public float floatStart;
    public float floatEnd;

    public Vector3 vector3Start;
    public Vector3 vector3End;


    private void FixedUpdate () {
        float delTime = Time.time - timebegin;


        if (delTime < time) {
            resultData.floatValue = (floatEnd - floatStart) * (delTime / time) + floatStart;
            resultData.vector3Value = (vector3End - vector3Start) * (delTime / time) + vector3Start;

            onFloatAnimation?.Invoke (resultData.floatValue);
            onVector2Animation?.Invoke (resultData.vector3Value);
        } else {
            resultData.floatValue = floatEnd;
            resultData.vector3Value = vector3End;

            onFloatAnimation?.Invoke (resultData.floatValue);
            onFloatAnimationEnd?.Invoke (resultData.floatValue);
            onVector2Animation?.Invoke (resultData.vector3Value);
            onVector2AnimationEnd?.Invoke (resultData.vector3Value);

            this.gameObject.Destroy ();
        }
    }


#if UNITY_EDITOR
    private void OnValidate () {

    }
#endif




    //*Public
    public static GameObject AnimateFloat (
        float floatStart, float floatEnd, float time, AnimationCurve curve = null,
        UnityAction<float> onAnimate = null, UnityAction<float> onAnimateEnd = null, Object createBy = null) {


        GameObject obj = new GameObject ("Animation Data");
        C_AnimateData comp = obj.AddComponent<C_AnimateData> ();
        comp.timebegin = Time.time;
        comp.time = time;
        comp.floatStart = floatStart;
        comp.floatEnd = floatEnd;
        if (curve != null) comp.curve = curve;
        comp.onFloatAnimation = onAnimate;
        comp.onFloatAnimationEnd = onAnimateEnd;
        comp.createBy = createBy;

        return obj;
    }
    public static GameObject AnimateVector2 (
        Vector2 vectorStart, Vector2 vectorEnd, float time, AnimationCurve curve = null,
        UnityAction<Vector2> onAnimate = null, UnityAction<Vector2> onAnimateEnd = null, Object createBy = null) {


        GameObject obj = new GameObject ("Animation Data");
        C_AnimateData comp = obj.AddComponent<C_AnimateData> ();
        comp.timebegin = Time.time;
        comp.time = time;
        comp.vector3Start = vectorStart;
        comp.vector3End = vectorEnd;
        if (curve != null) comp.curve = curve;
        comp.onVector2Animation = onAnimate;
        comp.onVector2AnimationEnd = onAnimateEnd;
        comp.createBy = createBy;

        return obj;
    }




    //*Property
    [System.Serializable]
    public class Test {
        public bool move = false;
    }

    [System.Serializable]
    public class Data {
        public float floatValue;
        public Vector3 vector3Value;
    }
}


public static class Extension_C_AnimateData {
    public static GameObject Ex_AnimateFloat (this Component obj,
            float floatStart, float floatEnd, float time, AnimationCurve curve = null,
            UnityAction<float> onAnimate = null,
            UnityAction<float> onAnimateEnd = null) =>


        C_AnimateData.AnimateFloat (floatStart, floatEnd, time, curve, onAnimate, onAnimateEnd, obj);

    public static GameObject Ex_AnimateVector2 (this Component obj,
        Vector2 start, Vector2 end, float time, AnimationCurve curve = null,
        UnityAction<Vector2> onAnimate = null,
        UnityAction<Vector2> onAnimateEnd = null) {

        GameObject gameObject = C_AnimateData.AnimateVector2 (start, end, time,
            curve, onAnimate, onAnimateEnd, obj);


        return gameObject;
    }

}