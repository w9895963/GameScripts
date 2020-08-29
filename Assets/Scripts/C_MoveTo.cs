using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_MoveTo : MonoBehaviour {
    public Vector2 targetPosition;
    public float time;
    public UnityAction callback;
    public AnimationCurve moveCurve = Fn.Curve.ZeroOneCurve;
    [ReadOnly] public float timebegin;
    [ReadOnly] public Vector2 beginPosition;
    [SerializeField, ReadOnly] private Object createBy;
    [SerializeField, ReadOnly] private bool arrived = true;
    public Test test = new Test ();

    private void FixedUpdate () {
        if (!arrived) {
            float delTime = Time.time - timebegin;
            Vector2 setP = transform.position;
            if (delTime < time) {
                setP = (targetPosition - beginPosition) * moveCurve.Evaluate (delTime / time) + beginPosition;
            } else {
                setP = targetPosition;
            }
            transform.Set2dPosition (setP);
            if (setP == targetPosition) {
                arrived = true;
                callback?.Invoke ();
            }
        }
    }



#if UNITY_EDITOR
    private void OnValidate () {
        if (test.move) {
            test.move = false;
            timebegin = Time.time;
            beginPosition = transform.position;
            arrived = false;
        }
    }
#endif




    //*Public
    public void Moveto (Vector2 targetPosition, float time = 0, UnityAction callback = null) {
        timebegin = Time.time;
        this.beginPosition = this.transform.position;
        this.targetPosition = targetPosition;
        this.time = time;
        this.callback = callback;
        this.arrived = false;
        if (time == 0) {
            transform.Set2dPosition (targetPosition);
            if (callback != null) callback ();
        }
    }
    public static void MoveTo (GameObject gameObject, Vector2 targetPosition,
        float time = 0, UnityAction callback = null, AnimationCurve moveCurve = default,
        Object createby = null) {


        Fn._.Destroy (gameObject.GetComponents<C_MoveTo> ());
        C_MoveTo moveComp = gameObject.AddComponent<C_MoveTo> ();
        if (moveCurve != default) {
            moveComp.moveCurve = moveCurve;
        }

        moveComp.createBy = createby;
        moveComp.Moveto (targetPosition, time, () => {
            if (callback != null) callback ();
            moveComp.Destroy ();
        });

    }



    //*Property
    [System.Serializable]
    public class Test {
        public bool move = false;
    }
}


public static class _Extiontion_C_MoveTo {
    public static void Ex_Moveto (this GameObject gameObject, Vector2 targetPosition,
            float time = 0, UnityAction callback = null, AnimationCurve moveCurve = default) =>


        C_MoveTo.MoveTo (gameObject, targetPosition, time, callback, moveCurve, gameObject);

}