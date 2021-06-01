using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Comp;
using UnityEngine;



public static class PrefabI
{
    public static Prefab UI_EditorToolBar = new Prefab("UI_EditorToolBar");
    public static Prefab UI_EditorItem_GroupTitle = new Prefab("UI_EditorItem_GroupTitle");
    public static Prefab UI_EditorItem_EditLine = new Prefab("UI_EditorItem_EditLine");
    public static Prefab UI_EditorItem_ClickTitle = new Prefab("UI_EditorItem_ClickTitle");
    public static Prefab UI_EditorItem_TitleTexture = new Prefab("UI_EditorItem_TitleTexture");
    public static Prefab UI_EditorItem_Selection = new Prefab("UI_EditorItem_Selection");
    public static Prefab UI_Editor = new Prefab("UI_Editor");

    public static Prefab BackgroundUI = new Prefab("BackgroundUI");


    public static Prefab SceneLayer = new Prefab("SceneLayer");
    public static Prefab DefaultLight = new Prefab("DefaultLight");



}


public static class PrefabF
{
    public static string GetPath(GameObject gameObject)
    {
        return gameObject.GetComponent<PrefabCom>().filePath;
    }

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








