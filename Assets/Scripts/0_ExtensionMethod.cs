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
    public static T[] Add<T> (this T[] source, int index, T newMember) {
        List<T> list = new List<T> (source);
        if (list.Count > index) {
            list[index] = newMember;
        } else {
            T[] ts = new T[index + 1];
            source.CopyTo (ts, 0);
            ts[index] = newMember;
            list = new List<T> (ts);
        }
        return list.ToArray ();
    }
    public static void Add<T> (this List<T> source, int index, T newMember) {
        if (source.Count <= index) {
            for (int i = source.Count; i < index + 1; i++) {
                source.Add (default);
            }
        }
        source[index] = newMember;
    }
    public static bool Contain<T> (this T[] source, T menber) {
        List<T> list = new List<T> (source);
        return list.Contains (menber);
    }
    public static bool Exist<T> (this T[] source, System.Predicate<T> match) {
        List<T> list = new List<T> (source);
        return list.Exists (match);
    }
    public static T Find<T> (this T[] source, System.Predicate<T> match) {
        List<T> list = new List<T> (source);
        return list.Find (match);
    }
    public static T[] ToArray<T> (this T source) {
        return new T[] { source };
    }
    public static void ForEach<T> (this T[] source, System.Action<T> action) {
        foreach (var t in source) {
            action (t);
        }
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
    public static Vector2 Rotate (this Vector2 vector, float angle) {
        return Quaternion.AngleAxis (angle, Vector3.forward) * vector;
    }


    public static float Min (this float f, float compareWith) {
        return f < compareWith ? f : compareWith;
    }
    public static float Min (this float f, params float[] compareWith) {
        List<float> l = new List<float> (compareWith);
        l.Add (f);
        l.Sort ();
        return l[0];
    }
    public static float Sign (this float f) {
        return Mathf.Sign (f);
    }
    public static float Abs (this float f) {
        return Mathf.Abs (f);
    }
    public static float Clamp (this float f, float min, float max) {
        return Mathf.Clamp (f, min, max);
    }



    public static EventTrigger Ex_AddInputToTrigger (this Collider2D collider,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {
        ///////////////////////////
        EventTrigger trigger = collider.gameObject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener (action);
        trigger.triggers.Add (entry);
        return trigger;
    }
    public static EventTrigger Ex_AddInputToTrigger (this GameObject gameobject,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {
        //////////////////////////////////////
        EventTrigger trigger = gameobject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener (action);
        trigger.triggers.Add (entry);
        return trigger;
    }

    public static EventTrigger Ex_AddInputToTriggerOnece (this Collider2D collider,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {



        EventTrigger trigger = collider.gameObject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener ((d) => {
            action (d);
            GameObject.Destroy (trigger);
        });
        trigger.triggers.Add (entry);
        return trigger;
    }
    public static EventTrigger Ex_AddInputToTriggerOnece (this Component comp,
        GameObject gameobject,
        EventTriggerType type,
        UnityAction<BaseEventData> action) {



        EventTrigger trigger = gameobject.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener ((d) => {
            action (d);
            GameObject.Destroy (trigger);
        });
        trigger.triggers.Add (entry);
        return trigger;
    }
    public static EventTrigger Ex_AddInputToTriggerOnece (this Component comp,
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




    public static void Destroy (this Object obj, float timeWait = 0) {
        if (timeWait <= 0) {
            GameObject.Destroy (obj);
        } else {
            GameObject.Destroy (obj, timeWait);
        }
    }
    public static void Destroy (this Component component) {
        GameObject.Destroy (component);
    }
    public static void Destroy (this List<Object> objects, params int[] indexs) {
        foreach (var i in indexs) {
            if (objects.Count > i) {
                GameObject.Destroy (objects[i]);
                objects[i] = null;
            }
        }
    }
    public static void Destroy (this List<Object> objects) {
        foreach (var obj in objects) {
            GameObject.Destroy (obj);
        }
    }
    public static void SetEnabled (this List<MonoBehaviour> component, bool enable) {
        foreach (var comp in component) {
            if (comp) {
                comp.enabled = enable;
            }
        }
    }
    public static T[] GetComponents<T> (this GameObject[] source) {
        List<T> list = new List<T> ();
        source.ForEach ((o) => {
            T t = o.GetComponent<T> ();
            if (t != null) {
                list.Add (t);
            }
        });
        return list.ToArray ();
    }
    public static T[] GetComponents<T> (this List<GameObject> source) {
        List<T> list = new List<T> ();
        source.ForEach ((o) => {
            T t = o.GetComponent<T> ();
            if (t != null) {
                list.Add (t);
            }
        });
        return list.ToArray ();
    }


    public static void Set2dPosition (this Transform transform, Vector2 position) {
        transform.position = new Vector3 (position.x, position.y, transform.position.z);
    }
    public static void SetTransparent (this SpriteRenderer render, float alpha) {
        Color color = render.color;
        render.color = new Color (color.r, color.g, color.b, alpha);
    }
    public static Vector2 Get2dPosition (this GameObject gameObject) {
        return (Vector2) gameObject.transform.position;
    }
    public static FixedJoint2D Ex_ConnectTo (this Rigidbody2D rigid, Rigidbody2D to,
        Vector2 fixedAnchor = default, Vector2 connectedAnchor = default
    ) { //--------------------------------//
        FixedJoint2D fixedJointComp = rigid.gameObject.AddComponent<FixedJoint2D> ();
        fixedJointComp.connectedBody = to;
        fixedJointComp.autoConfigureConnectedAnchor = false;
        fixedJointComp.connectedAnchor = connectedAnchor;
        fixedJointComp.anchor = fixedAnchor;
        return fixedJointComp;
    }




}