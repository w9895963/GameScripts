﻿using System.Collections;
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
    public static void DestroyImmediate(this Object obj)
    {
        GameObject.DestroyImmediate(obj);
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
    public static GameObject CreateChildFrom(this GameObject gameObject, string resourcePath, string name = null)
    {
        GameObject prefab = ResourceLoader.Load<GameObject>(resourcePath); ;
        GameObject obj = GameObject.Instantiate(prefab, gameObject.transform);
        if (name != null)
        {
            obj.name = name;
        }
        return obj;
    }
    public static List<GameObject> GetAllChild(this GameObject gameObject)
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
        return gameObject.GetAllChild().Find((x) => x.name == name);
    }
    public static bool HasChild(this GameObject gameObject, GameObject child)
    {
        List<GameObject> objs = gameObject.GetComponentsInChildren<Transform>().Select((t) => t.gameObject).ToList();
        return objs.Contains(child);
    }
    public static void SetParent(this GameObject gameObject, GameObject parent, bool stay = true)
    {
        gameObject.transform.SetParent(parent.transform, stay);
    }
    public static void SetParent(this GameObject gameObject, Component parent, bool stay = true)
    {
        gameObject.transform.SetParent(parent.transform, stay);
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
    public static T GetComponent<T>(this GameObject gameObject, bool autoAdd = false) where T : Component
    {

        T com = gameObject.GetComponent<T>();
        if (autoAdd == false)
        {
            return com;
        }
        if (com == null)
        {
            com = gameObject.AddComponent<T>();
        }
        return com;
    }






    public static void SetPositionLocal(this GameObject gameObject, Vector3 p)
    {
        Vector2 position = p;
        gameObject.transform.localPosition = p;
    }
    public static void SetPositionLocal(this GameObject gameObject, Vector2 p)
    {
        SetPositionLocal(gameObject, p.ToVector3(gameObject.transform.localPosition.z));
    }
    public static void SetPositionLocal(this GameObject gameObject, float x, float y)
    {
        SetPositionLocal(gameObject, new Vector3(x, y, gameObject.transform.localPosition.z));
    }
    public static void SetPositionLocal(this GameObject gameObject, float? x, float? y)
    {
        Vector3 l = gameObject.transform.localPosition;
        SetPositionLocal(gameObject, new Vector3(x ?? l.x, y ?? l.y, l.z));
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

    public static void AddPosition(this GameObject gameObject, Vector2 vector2)
    {
        gameObject.transform.position += new Vector3(vector2.x, vector2.y, 0);
    }
    public static void AddPosition(this GameObject gameObject, float x, float y)
    {
        AddPosition(gameObject, new Vector2(x, y));
    }

    public static void SetScale(this GameObject gameObject, Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }
    public static void SetScale(this GameObject gameObject, float scale)
    {

        Vector3 scale3 = new Vector3(scale, scale, scale);
        SetScale(gameObject, scale3);
    }
    public static void SetScale(this GameObject gameObject, float x, float y)
    {

        Vector3 scale = new Vector3(x, y, gameObject.transform.localScale.z);
        SetScale(gameObject, scale);
    }
    public static void SetScale(this GameObject gameObject, float? x, float? y)
    {

        Vector3 l = gameObject.transform.localScale;
        Vector3 scale = new Vector3(x ?? l.x, y ?? l.y, l.z);
        SetScale(gameObject, scale);
    }
    public static void SetScale(this GameObject gameObject, Vector2 scale)
    {
        Vector3 scale3 = new Vector3(scale.x, scale.y, gameObject.transform.localScale.z);
        SetScale(gameObject, scale3);
    }


    public static void SetRotate(this GameObject gameObject, float angle)
    {
        Quaternion rotation = gameObject.transform.localRotation;
        Vector3 angle3 = rotation.eulerAngles;
        angle3.z = angle;
        rotation.eulerAngles = angle3;
        gameObject.transform.rotation = rotation;
    }
    public static void SetRotate(this GameObject gameObject, Vector3 rotation)
    {
        Quaternion ro = new Quaternion();
        ro.eulerAngles = rotation;
        gameObject.transform.rotation = ro;
    }

    public static Vector2 GetPosition2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.position;
    }
    public static Vector2 GetPositionLocal2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.localPosition;
    }
    public static Vector2 GetScale2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.localScale;
    }
    public static float GetRotate1D(this GameObject gameObject)
    {
        return gameObject.transform.localRotation.eulerAngles.z;
    }
    public static Vector2 GetPositionBottomLeft(this GameObject gameObject)
    {
        Vector2 position = gameObject.transform.position;
        position = gameObject.GetComponent<Renderer>().bounds.min;
        return position;
    }


    public static Rigidbody2D GetRigidbody2D(this GameObject gameObject)
    {
        return gameObject.GetComponent<Rigidbody2D>();
    }





}