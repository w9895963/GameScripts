using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandInGameEditorBuilder : CommandActionHolder
        {
            const string editorPath = "CommandPrefab/Editor/InGameEditor";
            public override void Action(CommandLine commandLine)
            {
                
                var cl = commandLine;
                GameObject canvas = UIF.GetCanvas();
                gameObject.GetComponentInChildren<Editor.Editor>();
            }
        }
    }

}
