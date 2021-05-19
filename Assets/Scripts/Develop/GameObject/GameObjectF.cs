using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectF
{
    public static T FindComponentOrCreateObject<T>() where T : UnityEngine.Component
    {
        T comp = GameObject.FindObjectOfType<T>();
        if (comp == null)
        {
            comp = new GameObject(typeof(T).Name).AddComponent<T>();
        }
        return comp;
    }

    public static GameObject FindObjectByNameOrCreate(string name)
    {
        GameObject gameObject = GameObject.Find(name);
        if (gameObject == null)
        {
            gameObject = new GameObject(name);
        }
        return gameObject;
    }
    public static T FindObjectOfType<T>(string name = null) where T : UnityEngine.Object
    {
        if (name == null)
        {
            return GameObject.FindObjectOfType<T>();
        }
        else
        {
            T[] ts = GameObject.FindObjectsOfType<T>();
            T t = ts.ToList().Find((x) => x.name == name);
            return t;
        }

    }
    public static GameObject CreateFromPrefab(string path, string name = null)
    {
        GameObject prefab = ResourceLoader.Load<GameObject>(path);
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = name ?? obj.name;
        return obj;
    }

}
