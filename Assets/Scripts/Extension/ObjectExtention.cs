using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class ObjectExtention
{
    public static void Destroy(this List<Object> objects)
    {
        foreach (var obj in objects)
        {
            GameObject.Destroy(obj);
        }
        objects.RemoveRange(0, objects.Count);
    }
    public static void Destroy(this List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            GameObject.Destroy(obj);
        }
    }
    public static void Destroy(this Object obj, float timeWait = 0)
    {
        if (timeWait <= 0)
        {
            GameObject.Destroy(obj);
        }
        else
        {
            GameObject.Destroy(obj, timeWait);
        }
    }

    public static void Destroy(this Object[] objects)
    {
        foreach (var obj in objects)
        {
            GameObject.Destroy(obj);
        }
    }




    public static GameObject CreateChild(this GameObject gameObject, string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = gameObject.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        return obj;
    }
    public static GameObject CreateChild(this GameObject gameObject, GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate(prefab, gameObject.transform);
        return obj;
    }
    public static GameObject CreateChildFrom(this GameObject gameObject, string resourcePath)
    {
        GameObject prefab = Resources.Load(resourcePath, typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, gameObject.transform);
        return obj;
    }
    public static List<GameObject> FindAllChild(this GameObject gameObject)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            list.Add(gameObject.transform.GetChild(i).gameObject);
        }
        return list;
    }
    public static GameObject FindChild(this GameObject gameObject, string name)
    {
        return gameObject.FindAllChild().Find((x) => x.name == name);
    }
    public static bool HasChild(this GameObject gameObject, GameObject child)
    {
        List<GameObject> objs = gameObject.GetComponentsInChildren<Transform>().Select((t) => t.gameObject).ToList();
        return objs.Contains(child);
    }
    public static void SetParent(this GameObject gameObject, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
    }
    public static List<GameObject> GetParentsAndSelf(this GameObject gameObject)
    {
        List<GameObject> all = GetParents(gameObject);
        if (gameObject) all.Add(gameObject);
        return all;
    }
    public static List<GameObject> GetParents(this GameObject gameObject)
    {
        List<GameObject> all = new List<GameObject>();
        GameObject parent = GetParent(gameObject);
        while (parent != null)
        {
            all.Add(parent);
            parent = GetParent(parent);
        }
        return all;
    }
    public static GameObject GetParent(this GameObject gameObject)
    {
        Transform parent = gameObject == null ? null : gameObject.transform.parent;
        return parent ? parent.gameObject : null;
    }
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        T com = gameObject.GetComponent<T>();
        if (com == null)
        {
            com = gameObject.AddComponent<T>();
        }
        return com;
    }




    public static void SetPosition(this GameObject gameObject, Vector2 p)
    {
        Vector2 position = p;
        gameObject.transform.position = new Vector3(position.x, position.y, gameObject.transform.position.z);
    }
    public static void SetPosition(this GameObject gameObject, float x, float y)
    {
        Vector2 position = new Vector2(x, y);
        gameObject.transform.position = new Vector3(position.x, position.y, gameObject.transform.position.z);
    }
    public static void SetPosition(this GameObject gameObject, Vector3 p)
    {
        gameObject.transform.position = p;
    }
    public static void SetRotation(this GameObject gameObject, float angle)
    {
        Quaternion rotation = gameObject.transform.rotation;
        rotation.z = angle;
        gameObject.transform.rotation = rotation;
    }

    public static Vector2 GetPosition2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.position;
    }
    public static Vector2 GetPositionBottomLeft(this GameObject gameObject)
    {
        Vector2 position = gameObject.transform.position;
        position = gameObject.GetComponent<Renderer>().bounds.min;
        return position;
    }


    public static Rigidbody2D Rigidbody2D(this GameObject gameObject)
    {
        return gameObject.GetComponent<Rigidbody2D>();
    }





}