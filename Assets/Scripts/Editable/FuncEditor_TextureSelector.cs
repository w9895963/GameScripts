using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditableBundle.Comp;
using PrefabBundle;
using UIBundle;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EditableBundle
{

   

    namespace Func
    {
        public static class Editor_TextureSelector
        {
            static GameObject contentHolder => ShareDate.ContentHolder;

            static Prefab titlePrefab = PrefabI.UI_EditorItem_GroupTitle;

            static string toolName = "选择贴图";

            public static void ShowFolderTexture(UnityAction<string> onClick)
            {
                contentHolder.DestroyChildren();
                GameObject gameObject = titlePrefab.CreateInstance(contentHolder);
                gameObject.GetComponent<CompGroupTitle>().titleText = toolName;

                string[] allPath = FileF.GetAllFilesInLocalFolder("Texture", "*.png", true);
                allPath.ForEach((path) =>
                {
                    Texture2D texture2D = FileF.LoadTexture(path);

                    GameObject titleTextObj = PrefabI.UI_EditorItem_TitleTexture.CreateInstance();
                    CompItemTexture com = titleTextObj.GetComponent<CompItemTexture>();
                    com.titleText = FileF.GetFolderName(path) + "/" + Path.GetFileNameWithoutExtension(path);
                    com.SetTexture(texture2D);
                    com.AddOnClickAction(() => onClick(path));
                    titleTextObj.SetParent(contentHolder, false);

                });

            }


        }
    }



}