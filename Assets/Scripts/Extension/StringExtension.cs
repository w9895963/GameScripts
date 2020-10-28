using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtension {

    public static string InsertLast (this string str, string text) {
        return str.Insert (str.Length, text);
    }

}