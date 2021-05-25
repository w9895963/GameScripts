using System;
using System.Collections;
using System.Collections.Generic;
using CommandFileBundle;
using SceneBundle;
using UnityEngine;

public static class SceneF
{


    public static SceneHolder FindScene(string name)
    {
        return GameObjectF.FindObjectOfType<SceneBundle.SceneHolder>(name);
    }
    public static SceneHolder FindOrCreateScene(string name)
    {
        return GameObjectF.FindComponentOrCreate<SceneHolder>(name);
    }

    public static void AddToScene(GameObject obj, string sceneName)
    {
        var scene = GameObjectF.FindComponentOrCreate<SceneBundle.SceneHolder>(sceneName);
        obj?.SetParent(scene.gameObject);
    }


   
}
