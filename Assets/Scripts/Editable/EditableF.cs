using System.Collections;
using System.Collections.Generic;
using EditableBundle;
using PrefabBundle;
using UnityEngine;

public static class EditableF
{



    public static void ShowObjectEditor(GameObject gameObject)
    {
        EditableBundle.Func.Editor_Enable.EnableEditor();
        EditableBundle.Func.Editor_ListAllEditableDate.BuildEditorFor(gameObject);
    }


   
}
