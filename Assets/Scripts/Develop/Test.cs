using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Global;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public string path;

    private Rigidbody2D body => gameObject.GetRigidbody2D();

    private void Awake()
    {
    }
    private void Reset()
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
        GetComponent<Light>().renderingLayerMask.Log();
    }






}