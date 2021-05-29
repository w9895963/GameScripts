using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMDBundle
{

    public static class EditorBuilder
    {
        const string EditorPath = "CommandPrefab/Editor/InGameEditor";
        public static Editor.EditorUIBuilder Build(CommandFile commandFile, CommandLine commandLine = null)
        {
            var editor = GameObjectF.FindOrCretePrefab(EditorPath).GetComponentInChildren<Editor.EditorUIBuilder>();
            editor.TriggerCommandLine = commandLine;
            editor.Build(commandFile);
            return editor;
        }
    }


}
