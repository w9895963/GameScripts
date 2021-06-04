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
    namespace Func
    {
        public static class Editor_CreatePrefab
        {
            static GameObject contentHolder => ShareDate.ContentHolder;

            static Prefab clickableTitlePrefab = PrefabI.UI_EditorItem_ClickTitle;
            static Prefab titlePrefab = PrefabI.UI_EditorItem_GroupTitle;
            static List<Prefab> CreatePrefabs = new List<Prefab>()  {
                PrefabI.SceneLayer,
                PrefabI.LightManager,
            };
            static string toolName = "创建物体";

            public static void ShowCreateList()
            {
                contentHolder.DestroyChildren();


                GameObject title = titlePrefab.CreateInstance(contentHolder, (obj) =>
                {
                    CompGroupTitle com = obj.GetComponent<CompGroupTitle>();
                    com.titleText = toolName;
                });


                CreatePrefabs.ForEach((pref) =>
                {
                    clickableTitlePrefab.CreateInstance(contentHolder, (obj) =>
                    {
                        var com = obj.GetComponent<CompItemClickableTitle>();
                        com.titleText = pref.PrintName;
                        com.AddClickAction(() =>
                        {
                            GameObject gameObject = pref.CreateInstance();
                            gameObject.name = pref.PrintName;
                            gameObject.GetComponent<Comp.CompEditableObject>(true);
                            EditableF.ShowObjectEditor(gameObject);
                        });
                    });
                });

            }


        }
    }








}