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


    public static string FixPath(this string path)
    {
        path = path.Replace(@"/", "\\");
        return path;
    }

}