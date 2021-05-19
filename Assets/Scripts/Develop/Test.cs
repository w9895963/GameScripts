using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public string path;

    private Rigidbody2D body => gameObject.GetRigidbody2D();

    private void Awake()
    {
        // CommandFileBundle.FolderExecuter.ExecuteAllFiles("Scripts");
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
        // FilePath.Get(path);
        // DataFile.DataFolderPath.Log();
        // FileF.GetAllFilePathsInFolder(DataFile.DataFolderPath).LogEach(1);
        // CommandFileBundle.FolderExecuter.ExecuteAllFiles("Scripts");
    }






}