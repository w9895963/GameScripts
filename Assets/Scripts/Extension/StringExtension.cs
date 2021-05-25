using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtension
{

    public static string InsertLast(this string str, string text)
    {
        return str.Insert(str.Length, text);
    }

    public static Vector2 ToVector2(this string str, char splitter = ',')
    {
        string[] vs = str.Split(splitter);
        return new Vector2(float.Parse(vs[0]), float.Parse(vs[1]));
    }
    public static float? TryFloat(this string str)
    {
        float? re = null;
        float f;
        bool v = float.TryParse(str, out f);
        if (v)
        {
            re = f;
        }
        return re;
    }
    public static float?[] TryFloat(this string[] strAr)
    {
        int length = strAr.Length;
        float?[] re = new float?[length];
        for (int i = 0; i < length; i++)
        {
            float f;
            bool v = float.TryParse(strAr[i], out f);
            if (v)
            {
                re[i] = f;
            }
        }
     
        return re;
    }
    



    public static bool Contains(this string str, List<string> contents)
    {
        for (int i = 0; i < contents.Count; i++)
        {
            if (!str.Contains(contents[i]))
            {
                return false;
            }
        }

        return true;
    }

}