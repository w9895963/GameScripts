using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace EditableBundle
{
    namespace Comp
    {

        public class CompToolBarFunction : MonoBehaviour
        {
            public static void FindObject()
            {
                Func.Editor_SelectObject.BuiltSelection();
            }
            public static void CreatePrefab()
            {
                Func.Editor_CreatePrefab.ShowCreateList();
            }
            public static void EditorEnable(bool enabled)
            {
                if (enabled)
                {
                    Func.Editor_Enable.EnableEditor();
                }
                else
                {
                    Func.Editor_Enable.DisableEditor();
                }

            }
            public static void Save()
            {

                Func.SaveAndLOad.Save();


            }
            public static void Load()
            {

                Func.SaveAndLOad.Load();


            }

        }
    }





}
