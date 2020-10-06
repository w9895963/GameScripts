using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class ExtensionMethod {


    #region //*Array & List



    public static void Add<T> (this List<T> source, int index, T newMember) {
        if (source.Count <= index) {
            for (int i = source.Count; i < index + 1; i++) {
                source.Add (default);
            }
        }
        source[index] = newMember;
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
    public static T GetOrAdd<T> (this List<T> source, int index) where T : class, new () {
        for (int i = source.Count; i <= index; i++) {
            source.Add (new T ());
        }
        return source[index];
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




    #endregion //*endregion




    #region //*Vector


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
    public static Vector2 Rotate (this Vector2 vector, Vector2 to) {
        return to.normalized * vector.magnitude;
    }
    public static Vector2 Rotate (this Vector2 vector, Vector2 from, Vector2 to) {
        return Quaternion.FromToRotation (from, to) * vector;
    }
    public static Vector2 ClamMinOnDirection (this Vector2 vector, float min, Vector2 direction) {
        Vector2 vectorH = vector.Project (direction);
        Vector2 vectorV = vector.ProjectOnPlane (direction);
        Vector2 vectorHnorm = vectorH.Rotate (direction, Vector2.right);
        vectorHnorm.x = vectorHnorm.x.ClampMin (min);
        vectorH = vectorHnorm.Rotate (Vector2.right, direction);
        return vectorH + vectorV;
    }
    public static Vector2 ClamMax (this Vector2 vector, float max) {
        if (vector.magnitude >= max) {
            return vector.normalized * max;
        } else {
            return vector;
        }
    }
    public static bool IsSameSide (this Vector2 vector, Vector2 to) {
        return Vector2.Angle (vector, to) < 90;
    }

    public static Vector3 ToVector3 (this Vector2 vector, float z = 0) {
        return new Vector3 (vector.x, vector.y, z);
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

    #endregion


    #region //*Float



    public static float Min (this float f, float compareWith) {
        return f < compareWith ? f : compareWith;
    }
    public static float Min (this float f, params float[] compareWith) {
        List<float> l = new List<float> (compareWith);
        l.Add (f);
        l.Sort ();
        return l[0];
    }
    public static float Max (this float f, float compareWith) {
        return f > compareWith ? f : compareWith;
    }
    public static float Sign (this float f) {
        return Mathf.Sign (f);
    }
    public static float Abs (this float f) {
        return Mathf.Abs (f);
    }
    public static float Pow (this float f, float p) {
        return Mathf.Pow (f, p);
    }
    public static float Clamp (this float f, float min, float max) {
        return Mathf.Clamp (f, min, max);
    }
    public static float ClampMin (this float f, float min) {
        return f > min?f : min;
    }
    public static float ClampMax (this float f, float max) {
        return f < max?f : max;
    }
    public static Vector2 ToVector2 (this float fl) {
        return new Vector2 (fl, fl);
    }

    #endregion

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


    #region //*Unity Object


    public static void Destroy (this List<Object> objects) {
        foreach (var obj in objects) {
            GameObject.Destroy (obj);
        }
        objects.RemoveRange (0, objects.Count);
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
    public static void Destroy (this Object[] objects) {
        foreach (var obj in objects) {
            GameObject.Destroy (obj);
        }
    }



    public static GameObject AddChildren (this GameObject gameObject, string name) {
        GameObject obj = new GameObject (name);
        obj.transform.parent = gameObject.transform;
        return obj;

    }



    public static void Set2dPosition (this GameObject gameObject, Vector2 p) {
        Vector2 position = p;
        gameObject.transform.position = new Vector3 (position.x, position.y, gameObject.transform.position.z);
    }

    public static Vector2 Get2dPosition (this GameObject gameObject) {
        return (Vector2) gameObject.transform.position;
    }

    #endregion//*Endregion




    #region //*Collider2dExMethod
    public static Vector2? ClosestPointToLine (this Collider2DExMethod ex, Vector2 position, Vector2 direction) {
        Vector2? result = null;
        int sourceLayer = ex.source.gameObject.layer;
        ex.source.gameObject.layer = Layer.tempLayer.Index;


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

        ex.source.gameObject.layer = sourceLayer;
        return result;
    }

    #endregion //*End



    // * ---------------------------------- 
    public static GameObjectExMethod _Ex (this GameObject gameObject, Object callBy) {
        return new GameObjectExMethod (gameObject, callBy);
    }
    public static Collider2DExMethod _Ex (this Collider2D collider, Object callBy) {
        return new Collider2DExMethod (collider, callBy);
    }
    public static Rigid2dExMethod _Ex (this Rigidbody2D rigidbody, Object callBy) {
        return new Rigid2dExMethod (rigidbody, callBy);
    }

}

public class GameObjectExMethod {
    public GameObject gameObject;
    public Object callby;
    public GameObjectExMethod (GameObject gameObject, Object callBy) {
        this.gameObject = gameObject;
        this.callby = callBy;
    }
    // * ---------------------------------- 

}

public class Collider2DExMethod {
    public Collider2D source;
    public Object callby;
    public Collider2DExMethod (Collider2D collider, Object callby) {
        source = collider;
        this.callby = callby;
    }
    //*--------------------------

}
public class Rigid2dExMethod {
    public Rigidbody2D rigidbody;
    public Object callby;
    public Rigid2dExMethod (Rigidbody2D rigidbody, Object callby) {
        this.rigidbody = rigidbody;
        this.callby = callby;
    }
    //*--------------------------

}

public class Classname {

}