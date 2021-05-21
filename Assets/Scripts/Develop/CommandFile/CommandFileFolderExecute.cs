using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace CommandFileBundle
{
    public static class FolderExecuter
    {
        const string DefaultFolder = "Scripts\\OnLoad";
        const string ScriptFolder = "Scripts";
        public static void ExecuteAllFiles(string localPath)
        {
            string[] files = FileF.GetAllFilesInFolderFromLocal(localPath, "*.txt", true);
            if (files.IsEmpty())
            {
                return;
            }
            files.ForEach((file) =>
            {
                CommandFileBundle.CommandFile.Execute(file);
            });

        }

        public static void ExecuteAllScripts()
        {
            ExecuteAllFiles(ScriptFolder);
        }
        public static void ExecuteDefaultFolder()
        {
            ExecuteAllFiles(DefaultFolder);
        }
    }
}
