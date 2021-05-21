using System;
using System.Collections;
using System.Collections.Generic;
using CommandFileBundle;
using SceneBundle;
using UnityEngine;

public static class SceneF
{


    public static SceneBundle.SceneHolder FindScene(string name)
    {
        return GameObjectF.FindObjectOfType<SceneBundle.SceneHolder>(name);
    }

    public static void AddToScene(GameObject obj, string sceneName)
    {
        var scene = GameObjectF.FindComponentOrCreateObject<SceneBundle.SceneHolder>(sceneName);
        obj.SetParent(scene.gameObject);
    }




    public static void AddCommandLine(string folderName, CommandLine commandLine)
    {
        SceneHolder sceneHolder = GameObjectF.FindComponentOrCreateObject<SceneHolder>(folderName);
        sceneHolder.comandLines.Add(commandLine);
    }
}
