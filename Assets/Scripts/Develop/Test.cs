using System;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public Action cl;
    private void Awake()
    {

    }
    private void Reset()
    {
        Action a = cl ;
        Debug.Log(a);
    }




}