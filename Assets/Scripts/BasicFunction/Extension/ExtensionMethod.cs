using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class ExtensionMethod
{



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
    public static T Add<T>(this List<T> source, Action<T> beforeAction) where T : class, new()
    {
        T t = new T();
        beforeAction(t);
        source.Add(t);
        return t;
    }


    public static void AddNotHas<T>(this List<T> source, T newMember)
    {
        if (!source.Contains(newMember))
        {
            source.Add(newMember);
        }

    }


    public static void SortBy<T>(this List<T> source, IEnumerable<int> index)
    {
        var i = index.ToArray();
        var indexDic = source.ToDictionary((x, i) => (x, i));
        source.Sort((x, y) => i[indexDic[x]].CompareTo(i[indexDic[y]]));
    }


    public static bool IsEmpty<T>(this T source) where T : System.Collections.IEnumerable
    {
        if (source != null)
        {
            foreach (var item in source)
            {
                return false;
            }
        }

        return true;
    }


    public static List<T> RemoveNull<T>(this List<T> source)
    {
        source.RemoveAll((x) => x == null);
        return source;
    }



    public static List<T> ToList<T>(this T[] source)
    {
        if (source == null)
        {
            return new List<T>();
        }
        return new List<T>(source);
    }

    public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> source, System.Func<T, int, (K, V)> selector)
    {

        Dictionary<K, V> re = new Dictionary<K, V>();
        source.ForEach((x, i) =>
        {
            (K, V) p = selector(x, i);
            re.Add(p.Item1, p.Item2);
        });
        return re;
    }
    public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> source, System.Func<T, (K, V)> selector)
    {

        Dictionary<K, V> re = new Dictionary<K, V>();
        source.ForEach((x) =>
        {
            (K, V) p = selector(x);
            re.Add(p.Item1, p.Item2);
        });
        return re;
    }
    public static Dictionary<K, T> ToDictionary<K, T>(this IEnumerable<T> source, System.Func<T, K> selector)
    {

        Dictionary<K, T> re = new Dictionary<K, T>();
        source.ForEach((x) =>
        {
            re.Add(selector(x), x);
        });
        return re;
    }




    public static T GetPrevious<T>(this List<T> list, System.Predicate<T> predicate) where T : class
    {
        int v = list.FindIndex(predicate);
        if (v <= 0)
        {
            return null;
        }
        else
        {
            return list[v - 1];
        }
    }



    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (var t in enumeration)
        {
            action(t);
        }

    }
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
    {
        int ind = 0;
        foreach (var t in enumeration)
        {
            action(t, ind);
            ind++;
        }
    }


    public static bool TryGet<T>(this T[] array, int ind, out T t)
    {
        bool re = false;
        T tOUt = default;
        if (array != null)
        {
            if (array.Length > ind)
            {
                tOUt = array[ind];
                re = true;
            }
        }
        t = tOUt;
        return re;
    }





    #endregion //*endregion





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

    public static float Sqrt(this float f)
    {
        return Mathf.Sqrt(f);
    }
    public static float PowSafe(this float f, float p)
    {
        return Mathf.Pow(f.Abs(), p) * f.Sign();
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
    public static int Clamp(this int f, int min, int max)
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
    public static float ClampAbsMax(this float f, float max)
    {
        return f.Abs() < max.Abs() ? f : max.Abs() * f.Sign();
    }
    public static int ClampMax(this int i, int max)
    {
        return i < max ? i : max;
    }
    public static int ClampMin(this int i, int Min)
    {
        return i > Min ? i : Min;
    }

    public static float ZeroRid(this float f, bool negative = false, float value = 0.0001f)
    {
        return f != 0 ? f : (negative == false ? Mathf.Abs(value) : -Mathf.Abs(value));
    }



    public static Vector2 Map(this int f, int inputStart, int inputEnd, Vector2 outputStart, Vector2 outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
    }
    public static float Map(this float f, float inputStart, float inputEnd, float outputStart, float outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
    }
    public static Vector2 Map(this float f, float inputStart, float inputEnd, Vector2 outputStart, Vector2 outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
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
        return (int)Mathf.Ceil(f);
    }
    public static float Ceil(this float f, float step)
    {
        return Mathf.Ceil(f / step) * step;
    }

    public static bool ToBool(this float f)
    {
        return f > 0 ? true : false;
    }
    public static float[] ToArray(this float f)
    {
        return new float[1] { f };
    }



    public static Vector2 ToVector2(this float fl)
    {
        return new Vector2(fl, fl);
    }

    #endregion






    #region //*Bool
    public static float ToFloat(this bool boo)
    {
        return boo == true ? 1f : 0f;
    }
    public static bool Revert(this bool boo)
    {
        return boo == true ? false : true;
    }


    #endregion



    #region System.Object
    public static bool IsType<T>(this System.Object obj)
    {
        return obj.GetType() == typeof(T);
    }
    #endregion




    public static Sprite ToSprite(this Texture tex, float pixelsPerUnit = 20)
    {
        Texture2D tex2d = (Texture2D)tex;
        Rect rect = new Rect(0, 0, tex2d.width, tex2d.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex2d, rect, pivot, pixelsPerUnit);
        return sprite;
    }















}



