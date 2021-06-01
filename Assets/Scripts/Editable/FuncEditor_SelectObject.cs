using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditableBundle.Comp;
using PrefabBundle;
using UIBundle;
using UnityEngine;
using UnityEngine.UI;

namespace EditableBundle
{
    namespace Func
    {
        public static class Editor_SelectObject
        {
            static GameObject contentHolder => ShareDate.ContentHolder;

            static Prefab clickableTitlePrefab = PrefabI.UI_EditorItem_ClickTitle;
            static Prefab titlePrefab = PrefabI.UI_EditorItem_GroupTitle;
            static string toolName = "选择物体";

            public static void BuiltSelection()
            {
                contentHolder.DestroyChildren();

                CompEditableObject[] compEditableObjects = GameObject.FindObjectsOfType<Comp.CompEditableObject>();
                GameObject[] allInst = compEditableObjects.Select((x) => x.gameObject).ToArray();

                GameObject title = titlePrefab.CreateInstance(contentHolder);
                CompGroupTitle com = title.GetComponent<CompGroupTitle>();
                com.titleText = toolName;

                allInst.ForEach((inst) =>
                {
                    GameObject titleObj = clickableTitlePrefab.CreateInstance(contentHolder);
                    var com = titleObj.GetComponent<CompItemClickableTitle>();
                    com.titleText = inst.name;
                    com.AddClickAction(() =>
                    {
                        EditableF.ShowObjectEditor(inst);
                    });
                });

            }


        }

    }
  







}