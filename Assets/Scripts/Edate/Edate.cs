using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edate<T>
{
    public GameObject gameObject;
    public Func<T> dateGet;
    public Action<T> dateSet;


}

public class EPosition : Edate<Vector2?>
{
    public EPosition()
    {
        dateGet=() =>gameObject.GetPosition2d();
    }
}
public class Edate
{
    public Func<System.Object> dateGet;
    public Action<System.Object> dateSet;

    public static List<Edate> d = new List<Edate>();
    public static Edate<Vector2> Position = new Edate<Vector2>();

}
