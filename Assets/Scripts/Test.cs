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


    }

    private void Awake()
    {
    }
    private void Reset()
    {

    }
    private void Start()
    {

        // PrefabI.UI_Canvas.CreateInstance();
        // PrefabBundle.Component.PrefabCom[] prefabComs = GameObject.FindObjectsOfType<PrefabBundle.Component.PrefabCom>(true);
        // prefabComs.LogEach();
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