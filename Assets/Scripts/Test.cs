﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Test : MonoBehaviour
{
    public string path;
    public GameObject obj;

    private Rigidbody2D body => gameObject.GetRigidbody2D();

    private void Awake()
    {
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


    [ContextMenu("Test")]
    void DoSomething()
    {
    }






}