using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace CommandFileBundle
{
    public static class FileReader
    {
        const string DefaultFolder = "Scripts\\OnLoad";
        const string ScriptFolder = "Scripts";
        public static void AddAllCommandLinesToScene(string localPath)
        {
            string[] paths = FileF.GetAllFilesInFolderFromLocal(localPath, "*.txt", true);
            if (paths.IsEmpty())
            {
                return;
            }
            paths.ForEach((path) =>
            {
                CommandFile commandFile = CommandFileBundle.CommandFile.TryGetCommandFile(path);
                 if (commandFile == null){ return ; }
                 commandFile.AddCommandLinesToScene();
            });

        }

        public static void ReadAllScriptsThenAddToScene()
        {
            AddAllCommandLinesToScene(ScriptFolder);
        }
       
    }
}
