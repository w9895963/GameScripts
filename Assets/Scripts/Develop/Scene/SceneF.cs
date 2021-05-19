using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneF
{
    public static GameObject CreateScene(string name)
    {
        GameObject scene = new GameObject(name, typeof(SceneBundle.SceneHolder));
        return scene;
    }

    public static void AddToScene(GameObject obj, string sceneName)
    {
        if (obj.GetComponent<SceneBundle.Scene_NotAddToSceneMark>() != null)
        {
            return;
        }
        var scene = GameObjectF.FindObjectOfType<SceneBundle.SceneHolder>(sceneName);
        if (scene != null)
        {
            obj.SetParent(scene.gameObject);
        }
    }
}
