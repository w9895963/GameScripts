using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    public static bool Contain<T> (this T[] source, T menber) {
        List<T> list = new List<T> (source);
        return list.Contains (menber);
    }
    public static bool Exist<T> (this T[] source, System.Predicate<T> match) {
        List<T> list = new List<T> (source);
        return list.Exists (match);
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


    public static float Sign (this float f) {
        return Mathf.Sign (f);
    }


    public static EventTrigger Ex_AddInputToTrigger (this Component comp,
        Collider2D targetCollider,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {



        EventTrigger trigger = targetCollider.gameObject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener (action);
        trigger.triggers.Add (entry);
        return trigger;
    }

    public static EventTrigger Ex_AddInputEventToTriggerOnece (this Component comp,
        Collider2D targetCollider,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {



        EventTrigger trigger = targetCollider.gameObject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener ((d) => {
            action (d);
            GameObject.Destroy (trigger);
        });
        trigger.triggers.Add (entry);
        return trigger;
    }

}