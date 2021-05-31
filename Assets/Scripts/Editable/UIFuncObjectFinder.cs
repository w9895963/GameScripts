using System;
using System.Collections;
using System.Collections.Generic;
using EditableBundle.Comp;
using PrefabBundle;
using UIBundle;
using UnityEngine;
using UnityEngine.UI;

namespace EditableBundle
{

    public static class UIFuncObjectFinder
    {
        static GameObject contentHolder => ShareField.ContentHolder;

        static Prefab clickableTitlePrefab = PrefabI.UI_EditorItem_ClickTitle;
        static Prefab titlePrefab = PrefabI.UI_EditorItem_Title;
        static List<Prefab> allPrefabs = new List<Prefab>()
        {
            PrefabI.SceneLayer,
        };
        static string toolName = "选择物体";

        public static void BuiltSelection()
        {
            contentHolder.DestroyChildren();

            GameObject[] allInst = PrefabF.FindAllInstances(allPrefabs);

            GameObject title = titlePrefab.CreateInstance(contentHolder);
            CompTitle com = title.GetComponent<CompTitle>();
            com.titleText = toolName;

            allInst.ForEach((inst) =>
            {
                GameObject titleObj = clickableTitlePrefab.CreateInstance(contentHolder);
                var com = titleObj.GetComponent<CompClickableTitle>();
                com.titleText = inst.name;
                com.AddOnClickAction(() =>
                {
                    EditableF.ShowObjectEditor(inst);
                });
            });

        }


    }







}