using System.Collections.Generic;
using Global;
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



    //*Array & List
    public static void Add<T> (this List<T> source, int index, T newMember) {
        if (source.Count <= index) {
            for (int i = source.Count; i < index + 1; i++) {
                source.Add (default);
            }
        }
        source[index] = newMember;
    }
    public static T[] RemoveAll<T> (this T[] source, System.Predicate<T> match) {
        List<T> lists = source.ToList ();
        lists.RemoveAll (match);
        return lists.ToArray ();
    }

    public static List<T> ExpendTo<T> (this List<T> source, int index) where T : new () {
        if (source.Count <= index) {
            source.Add (index, new T ());
        }
        return source;

    }
    public static void AddNotHas<T> (this List<T> source, T newMember) {
        if (!source.Contains (newMember)) {
            source.Add (newMember);
        }

    }
    public static void AddNotHas<T> (this List<T> source, List<T> newMembers) {
        newMembers.ForEach ((x) => {
            if (!source.Contains (x)) {
                source.Add (x);
            }
        });
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
    public static List<T> ToList<T> (this T[] source) {
        return new List<T> (source);
    }
    public static void ForEach<T> (this T[] source, System.Action<T> action) {
        foreach (var t in source) {
            action (t);
        }
    }


    //* Vector2

    public static Vector2 ProjectOnPlane (this Vector2 vector, Vector2 normal) {
        return (Vector2) Vector3.ProjectOnPlane (vector, normal);
    }
    public static Vector2 Project (this Vector2 vector, Vector2 direction) {
        return (Vector2) Vector3.Project (vector, direction);
    }
    public static Vector2 ScreenToWold (this Vector2 vector) {
        return Camera.main.ScreenToWorldPoint (vector);
    }
    public static Vector2 WoldToScreen (this Vector2 vector) {
        return Camera.main.WorldToScreenPoint (vector);
    }
    public static Vector2 Rotate (this Vector2 vector, float angle) {
        return Quaternion.AngleAxis (angle, Vector3.forward) * vector;
    }
    public static Vector2 Reverse (this Vector2 vector) {
        return vector * -1;
    }

    //*Vector2?
    public static Vector2 ToVector2 (this Vector2? vector) {
        return vector == null?Vector2.zero: (Vector2) vector;
    }
    public static bool NotNull (this Vector2? vector) {
        return vector == null?false : true;
    }
    public static bool IsNull (this Vector2? vector) {
        return vector == null?true : false;
    }

    //*Vector3
    public static Vector2 ToVector2 (this Vector3 vector) {
        return (Vector2) vector;
    }
    public static Vector2 ToVector2 (this Vector3? vector) {
        return vector != null?(Vector2) vector : Vector2.zero;
    }



    //*Float
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
    public static Vector2 ToVector2 (this float fl) {
        return new Vector2 (fl, fl);
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
    public static void Destroy (this Object[] objects) {
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
    public static T[] GetComponents<T> (this GameObject[] gameObject) {
        List<T> list = new List<T> ();
        gameObject.ForEach ((o) => {
            T t = o.GetComponent<T> ();
            if (t != null) {
                list.Add (t);
            }
        });
        return list.ToArray ();
    }
    public static T[] GetComponents<T> (this List<GameObject> gameObjectList) {
        List<T> list = new List<T> ();
        gameObjectList.ForEach ((o) => {
            T t = o.GetComponent<T> ();
            if (t != null) {
                list.Add (t);
            }
        });
        return list.ToArray ();
    }

    public static GameObject AddChildren (this GameObject gameObject, string name) {
        GameObject obj = new GameObject (name);
        obj.transform.parent = gameObject.transform;
        return obj;

    }


    public static void Set2dPosition (this Transform transform, Vector2 position) {
        transform.position = new Vector3 (position.x, position.y, transform.position.z);
    }
    public static void Set2dPosition (this GameObject gameObject, Vector2? p) {
        Vector2 position = p.ToVector2 ();
        gameObject.transform.position = new Vector3 (position.x, position.y, gameObject.transform.position.z);
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

    //*--------------------------------------------------------
    public static GameObjectExMethod _ExMethod (this GameObject gameObject) {
        return new GameObjectExMethod (gameObject);
    }



    public static Collider2dExMethod _ExMethod (this Collider2D collider) {
        return new Collider2dExMethod (collider);
    }

}

namespace Global {
    public class GameObjectExMethod {
        public GameObject gameObject;
        public GameObjectExMethod (GameObject gameObject) {
            this.gameObject = gameObject;
        }
    }

    public class Collider2dExMethod {
        private Collider2D source;
        public Collider2dExMethod (Collider2D collider) {
            source = collider;
        }
        //*--------------------------
        public Vector2? ClosestPointToLine (Vector2 position, Vector2 direction) {
            Vector2? result = null;
            int sourceLayer = source.gameObject.layer;
            source.gameObject.layer = Layer.tempLayer.Index;


            RaycastHit2D hit;
            hit = Physics2D.Raycast (position, direction, Mathf.Infinity, Layer.tempLayer.Mask);
            if (hit != default) {
                result = hit.point;
            } else {
                hit = Physics2D.Raycast (position, -direction, Mathf.Infinity, Layer.tempLayer.Mask);
                if (hit != default) {
                    result = hit.point;
                }
            }

            source.gameObject.layer = sourceLayer;
            return result;
        }
    }

}