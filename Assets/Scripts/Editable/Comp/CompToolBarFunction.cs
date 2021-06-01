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
        }
    }





}
