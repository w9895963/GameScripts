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


    public struct TestClass
    {
        public string name;

        public TestClass(string name)
        {
            this.name = name;
        }
    }

    [ContextMenu("Test")]
    void Test1()
    {
        EditableF.ShowObjectEditor(PrefabI.EditableSetting.Find());

    }
    [ContextMenu("Test2")]
    void Test2()
    {

        EditableBundle.Func.SaveAndLOad.Load();
    }
    [ContextMenu("Test3")]
    void Test3()
    {

        FileBundle.LocalFile.GetLocalPath(str).Log();
    }

    private void Awake()
    {
        testKey.performed += (d) =>
        {
            Test1();
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