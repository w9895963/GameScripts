using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public InputAction testKey = new InputAction("TestKey", InputActionType.Button, "<Keyboard>/f11");
    public string str;
    public GameObject obj;
    public GameObject obj2;
    public List<int> d;
    public Vector2 v;
    Func<string> act;
    System.Object o;



    [ContextMenu("Test")]
    void DoSomething()
    {

        EditableBundle.Func.SaveAllDate.Save();
    }

    private void Awake()
    {
        testKey.performed += (d) =>
        {
            DoSomething();
        };
        testKey.Enable();
    }
    private void Reset()
    {

    }
    private void Start()
    {


    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {

    }



    private void FixedUpdate()
    {

    }

    private void OnValidate()
    {
    }









}