using System.Collections;
using System.Collections.Generic;
using EditableBundle.DateType;
using PrefabBundle;
using UnityEngine;

namespace EditableBundle
{
    public static class ShareDate
    {
        static Prefab EditorPrefab = PrefabI.UI_Editor;
        public static GameObject Editor => PrefabF.FindOrCretePrefab(EditorPrefab);
        public static GameObject ContentHolder => Editor.GetComponentInChildren<Comp.CompEditorContentHolder>().gameObject;


        public enum EditableLightType
        {
            GlobalLight,
            PointLight,
            NormalLight,
        }


        public static List<EditDate> AllEditDate => new List<EditDate>()
        {
            new Editable_Name(),
            new Editable_Position(),
            new Editable_Scale(),
            new Editable_SortLayer(),
            new Editable_Sprite(),

            new Light_LayerAndType(),
        };
    }
}
