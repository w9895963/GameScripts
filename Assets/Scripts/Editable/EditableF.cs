using System.Collections;
using System.Collections.Generic;
using EditableBundle;
using PrefabBundle;
using UnityEngine;

public static class EditableF
{



    public static void ShowObjectEditor(GameObject gameObject)
    {
        EditableBundle.UIFunc.EnableEditor();
        EditableBundle.ObjectEditorBuilder.BuildEditorFor(gameObject);
    }
}
