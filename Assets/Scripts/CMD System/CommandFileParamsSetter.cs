using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    public static class Setter
    {
        public static void Set(string param, Action<float> setter)
        {
            setter.Invoke(float.Parse(param));
        }
    }
}