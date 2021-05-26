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
        Dropdown dropdown = gameObject.GetComponentInChildren<Dropdown>();
        var ip = gameObject.GetComponentInChildren<InputField>();
        InputField inputField = obj.GetComponent<InputField>();
        ip.onEndEdit.AddListener((s) =>
        {
            dropdown.Show();
            dropdown.Select();
        });

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