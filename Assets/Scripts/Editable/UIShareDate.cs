using System.Collections;
using System.Collections.Generic;
using PrefabBundle;
using UnityEngine;

namespace EditableBundle
{
    public static class ShareField
    {
        static Prefab EditorPrefab = PrefabI.UI_Editor;
        public static GameObject Editor => PrefabF.FindOrCretePrefab(EditorPrefab);
        public static GameObject ContentHolder => Editor.GetComponentInChildren<Comp.MarkEditorContentHolder>().gameObject;
    }
}
