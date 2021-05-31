using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Component;
using UnityEngine;



public static class PrefabI
{
    public static Prefab UI_Editor_Table = new Prefab("UI_Editor_Table");
    public static Prefab UI_EditorToolBar = new Prefab("UI_EditorToolBar");
    public static Prefab UI_EditorItem_Title = new Prefab("UI_EditorItem_Title");
    public static Prefab UI_EditorItem_EditLine = new Prefab("UI_EditorItem_EditLine");
    public static Prefab UI_EditorItem_ClickTitle = new Prefab("UI_EditorItem_ClickTitle");
    public static Prefab UI_Canvas = new Prefab("UI_Canvas");
    public static Prefab UI_Editor = new Prefab("UI_Editor");

    public static Prefab BackgroundUI = new Prefab("BackgroundUI");


    public static Prefab SceneLayer = new Prefab("SceneLayer");



}


public static class PrefabF
{

    public static GameObject FindOrCretePrefab(Prefab prefab)
    {
        GameObject gameObject = prefab.Find();
        if (gameObject == null)
        {
            gameObject = prefab.CreateInstance();
        }
        return gameObject;
    }

    public static GameObject[] FindAllInstances(IEnumerable<Prefab> prefabs)
    {
        List<GameObject> re = new List<GameObject>();
        foreach (var pref in prefabs)
        {
            re.AddRange(pref.FindAll());
        }


        return re.ToArray();
    }
}








