using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_AnimateData : MonoBehaviour {
    public float time;
    public AnimationCurve curve = Fn.Curve.ZeroOneCurve;
    public UnityAction<Data> onAnimationEnd;
    public UnityAction<Data> onAnimation;
    public Data data = new Data ();
    [ReadOnly] public float timebegin;
    [SerializeField, ReadOnly] private Object createBy = null;
    public Test test = new Test ();
    public float floatStart;
    public float floatEnd;

    private void FixedUpdate () {
        float delTime = Time.time - timebegin;

        Vector2 setP = transform.position;

        if (delTime < time) {
            data.floatValue = (floatEnd - floatStart) * (delTime / time) + floatStart;
            onAnimation?.Invoke (data);
        } else {
            data.floatValue = floatEnd;
            onAnimation?.Invoke (data);
            onAnimationEnd?.Invoke (data);
            this.gameObject.Destroy ();
        }
    }


#if UNITY_EDITOR
    private void OnValidate () {

    }
#endif




    //*Public
    public static GameObject AnimateFloat (
        float floatStart, float floatEnd, float time, AnimationCurve curve,
        UnityAction<Data> onAnimate = null, UnityAction<Data> onAnimateEnd = null, Object createBy = null) {


        GameObject obj = new GameObject ("Animation Data");
        C_AnimateData comp = obj.AddComponent<C_AnimateData> ();
        comp.timebegin = Time.time;
        comp.time = time;
        comp.floatStart = floatStart;
        comp.floatEnd = floatEnd;
        if (curve != null) comp.curve = curve;
        comp.onAnimation = onAnimate;
        comp.onAnimationEnd = onAnimateEnd;
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
    }
}


public static class _Extiontion_C_AnimateData {
    public static GameObject Ex_AnimateFloat (this GameObject obj,
            float floatStart, float floatEnd, float time, AnimationCurve curve = null,
            UnityAction<C_AnimateData.Data> onAnimate = null,
            UnityAction<C_AnimateData.Data> onAnimateEnd = null) =>


        C_AnimateData.AnimateFloat (floatStart, floatEnd, time, curve, onAnimate, onAnimateEnd, obj);

}