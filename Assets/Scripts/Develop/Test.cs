using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public int i = 0;
    private float currP;
    public float dist;
    private float? time;

    private Rigidbody2D body => gameObject.Rigidbody2D();

    private void Awake()
    {
        // body.velocity = Vector2.up * 10;
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






}