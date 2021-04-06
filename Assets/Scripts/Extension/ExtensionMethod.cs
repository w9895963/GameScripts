using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class ExtensionMethod
{
    public static EventTrigger Ex_AddInputToTrigger(this Collider2D collider,
        EventTriggerType type,
        UnityAction<BaseEventData> action)
    {
        ///////////////////////////
        EventTrigger trigger = collider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
        return trigger;
    }


    #region //*Array & List

    public static void Add<T>(this List<T> source, int index, T newMember)
    {
        if (source.Count <= index)
        {
            for (int i = source.Count; i < index + 1; i++)
            {
                source.Add(default);
            }
        }
        source[index] = newMember;
    }
    public static void AddNotHas<T>(this List<T> source, T newMember)
    {
        if (!source.Contains(newMember))
        {
            source.Add(newMember);
        }

    }
    public static void Sort<T>(this List<T> source, System.Func<T, float> selector)
    {
        source.Sort((x, y) => selector(x).CompareTo(selector(y)));
    }

    public static bool NotEmpty<T>(this List<T> source)
    {
        if (source != null)
        {
            if (source.Count > 0)
            {
                return true;
            }
        }
        return false;
    }


    public static List<T> ExpendTo<T>(this List<T> source, int index) where T : new()
    {
        if (source.Count <= index)
        {
            source.Add(index, new T());
        }
        return source;

    }


    public static T[] ToArray<T>(this T source)
    {
        return new T[] { source };
    }
    public static List<T> ToList<T>(this T[] source)
    {
        return new List<T>(source);
    }
    public static List<T> ToList<T>(this T source) where T : Object
    {
        return new List<T> { source };
    }
    public static List<string> ToList(this string source)
    {
        return new List<string> { source };
    }


    public static void ForEach<T>(this T[] source, System.Action<T> action)
    {
        foreach (var t in source)
        {
            action(t);
        }
    }

    public static void LogAll<T>(this List<T> source)
    {
        for (int i = 0; i < source.Count; i++)
        {
            Debug.Log($"index:{i};content:{source[i]}", source[i] as UnityEngine.Object);
        }
    }
    public static void LogAll<T, S>(this List<T> source, System.Func<T, S> selector)
    {
        List<S> lists = source.Select(selector).ToList();
        for (int i = 0; i < lists.Count; i++)
        {
            Debug.Log(lists[i], lists[i] as UnityEngine.Object);
        }
    }




    #endregion //*endregion




    #region //*Vector


    public static Vector2 ProjectOnPlane(this Vector2 vector, Vector2 normal)
    {
        return (Vector2)Vector3.ProjectOnPlane(vector, normal);
    }
    public static Vector2 Project(this Vector2 vector, Vector2 direction)
    {
        return (Vector2)Vector3.Project(vector, direction);
    }

    public static float ProjectToFloat(this Vector2 vector, Vector2 direction)
    {
        Vector2 vct = (Vector2)Vector3.Project(vector, direction);
        float result = vct.magnitude;
        if ((vct.normalized + direction.normalized).magnitude < 1)
            result *= -1;
        return result;
    }
    public static Vector2 ScreenToWold(this Vector2 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }
    public static Vector2 WoldToScreen(this Vector2 vector)
    {
        return Camera.main.WorldToScreenPoint(vector);
    }
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * vector;
    }
    public static Vector2 Rotate(this Vector2 vector, Vector2 to)
    {
        return to.normalized * vector.magnitude;
    }
    public static Vector2 Rotate(this Vector2 vector, Vector2 from, Vector2 to)
    {
        return Quaternion.FromToRotation(from, to) * vector;
    }
    public static float Angle(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to);
    }
    public static Vector2 ClamMinOnDirection(this Vector2 vector, float min, Vector2 direction)
    {
        Vector2 vectorH = vector.Project(direction);
        Vector2 vectorV = vector.ProjectOnPlane(direction);
        Vector2 vectorHnorm = vectorH.Rotate(direction, Vector2.right);
        vectorHnorm.x = vectorHnorm.x.ClampMin(min);
        vectorH = vectorHnorm.Rotate(Vector2.right, direction);
        return vectorH + vectorV;
    }
    public static Vector2 ClampMax(this Vector2 vector, float max)
    {
        if (vector.magnitude >= max)
        {
            return vector.normalized * max;
        }
        else
        {
            return vector;
        }
    }
    public static bool IsSameSide(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to) < 90;
    }

    public static Vector3 ToVector3(this Vector2 vector, float z = 0)
    {
        return new Vector3(vector.x, vector.y, z);
    }


    //*Vector2?
    public static Vector2 ToVector2(this Vector2? vector)
    {
        return vector == null ? Vector2.zero : (Vector2)vector;
    }
    public static bool NotNull(this Vector2? vector)
    {
        return vector == null ? false : true;
    }
    public static bool IsNull(this Vector2? vector)
    {
        return vector == null ? true : false;
    }

    //*Vector3
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return (Vector2)vector;
    }
    public static Vector2 ToVector2(this Vector3? vector)
    {
        return vector != null ? (Vector2)vector : Vector2.zero;
    }

    //* Vector List

    public static bool IsSame(this List<Vector2> source, List<Vector2> list)
    {
        bool result = true;
        List<Vector2> l1 = new List<Vector2>(source);
        List<Vector2> l2 = new List<Vector2>(list);
        if (l1.Count != l2.Count)
        {
            result = false;
        }
        else
        {
            l1.RemoveAll((x) => list.Contains(x));
            l2.RemoveAll((x) => source.Contains(x));
            if (l1.Count != 0 | l2.Count != 0)
            {
                result = false;
            }
        }
        return result;
    }

    #endregion




    #region //*Float & Int





    public static float Min(this float f, float compareWith)
    {
        return f < compareWith ? f : compareWith;
    }
    public static float Sign(this float f)
    {
        return Mathf.Sign(f);
    }
    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
    }
    public static float Pow(this float f, float p)
    {
        return Mathf.Pow(f, p);
    }
    public static float Shape(this float f, float pow, float move = 0, float div = 1)
    {
        float abs = Mathf.Abs(f);
        float sign = Mathf.Sign(f);
        float p1 = (Mathf.Pow((abs + move) / div, pow) * div) - move;
        return p1 * sign;
    }



    public static float Clamp(this float f, float min, float max)
    {
        return Mathf.Clamp(f, min, max);
    }
    public static float ClampMin(this float f, float min)
    {
        return f > min ? f : min;
    }
    public static float ClampAbsMin(this float f, float min)
    {
        float fas = Mathf.Abs(f);
        float fsi = Mathf.Sign(f);
        fas = fas < min ? min : fas;
        return fas * fsi;
    }
    public static float ClampMax(this float f, float max)
    {
        return f < max ? f : max;
    }
    public static int ClampMax(this int i, int max)
    {
        return i < max ? i : max;
    }
    public static float Map(this float f, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputMin, inputMax);
        return (f1 - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }
    public static float Floor(this float f)
    {
        return Mathf.Floor(f);
    }
    public static int FloorToInt(this float f)
    {
        return Mathf.FloorToInt(f);
    }
    public static float Ceil(this float f)
    {
        return Mathf.Ceil(f);
    }
    public static int CeilToInt(this float f)
    {
        return Mathf.CeilToInt(f);
    }
    public static bool ToBool(this float f)
    {
        return f == 1 ? true : false;
    }
    


    public static Vector2 ToVector2(this float fl)
    {
        return new Vector2(fl, fl);
    }

    #endregion


    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }



    #region //*Bool
    public static float ToFloat(this bool boo)
    {
        return boo == true ? 1f : 0f;
    }
    #endregion





    #region //* Other
    public static bool IsList(this System.Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }
    #endregion




    #region //*Collider2dExMethod
    public static Vector2? ClosestPointToLine(this Collider2DExMethod ex, Vector2 position, Vector2 direction)
    {
        Vector2? result = null;
        int sourceLayer = ex.collider.gameObject.layer;
        ex.collider.gameObject.layer = LayerUtility.temp.Index;


        RaycastHit2D hit;
        hit = Physics2D.Raycast(position, direction, Mathf.Infinity, LayerUtility.temp.Mask);
        if (hit != default)
        {
            result = hit.point;
        }
        else
        {
            hit = Physics2D.Raycast(position, -direction, Mathf.Infinity, LayerUtility.temp.Mask);
            if (hit != default)
            {
                result = hit.point;
            }
        }

        ex.collider.gameObject.layer = sourceLayer;
        return result;
    }
    public static (EventTrigger, EventTrigger.Entry) AddPointerEvent(this Collider2DExMethod souce,
            EventTriggerType type, UnityAction<BaseEventData> action
        ) =>
        souce.collider.gameObject._Ex(default).AddPointerEvent(type, action);
    #endregion //*End




    #region //*GameObjectExMethod
    public static (EventTrigger, EventTrigger.Entry) AddPointerEvent(this GameObjectExMethod souce,
        EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // * ---------------------------------- 
        EventTrigger trigger = souce.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = souce.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
        return (trigger, entry);
    }
    #endregion




    // * ---------------------------------- 
    public static GameObjectExMethod _Ex(this GameObject gameObject, Object callBy)
    {
        return new GameObjectExMethod(gameObject, callBy);
    }
    public static Collider2DExMethod _Ex(this Collider2D collider, Object callBy)
    {
        return new Collider2DExMethod(collider, callBy);
    }
    public static RigidBody2dExMethod _Ex(this Rigidbody2D rigidbody, Object callBy)
    {
        return new RigidBody2dExMethod(rigidbody, callBy);
    }

}

public class GameObjectExMethod
{
    public GameObject gameObject;
    public Object callby;
    public GameObjectExMethod(GameObject gameObject, Object callBy)
    {
        this.gameObject = gameObject;
        this.callby = callBy;
    }
    // * ---------------------------------- 

}
public class Collider2DExMethod
{
    public Collider2D collider;
    public Object callby;
    public Collider2DExMethod(Collider2D collider, Object callby)
    {
        this.collider = collider;
        this.callby = callby;
    }
    //*--------------------------

}
public class RigidBody2dExMethod
{
    public Rigidbody2D rigidbody;
    public Object callby;
    public RigidBody2dExMethod(Rigidbody2D rigidbody, Object callby)
    {
        this.rigidbody = rigidbody;
        this.callby = callby;
    }
    //*--------------------------

}