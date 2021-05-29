using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMDBundle
{
    namespace Editor
    {
        public class EditorUIBuilder : MonoBehaviour
        {

            public CommandLine TriggerCommandLine;
            public GameObject dynamicHeight;



            public void Build(CommandFile commandFile)
            {
                var editorTitle = gameObject.GetComponentInChildren<FileTitleUIBuilder>();
                editorTitle.editor = this;
                editorTitle.commandFile = commandFile;
                editorTitle.Initial();


                CommandLineUIBuilder linePrefabCom = gameObject.GetComponentInChildren<CommandLineUIBuilder>();
                commandFile.commandLines.ForEach((line) =>
                {
                    if (line.Empty) return;
                    CommandLineUIBuilder lineCom = GameObject.Instantiate(linePrefabCom.gameObject, linePrefabCom.transform.parent).GetComponent<CommandLineUIBuilder>();
                    lineCom.cl = line;
                    lineCom.Initial();
                });
                linePrefabCom.gameObject.SetActive(false);
                dynamicHeight?.SetHeight(UIF.GetChildrenHeight(linePrefabCom.gameObject.GetParent()));
            }
        }
    }

}
