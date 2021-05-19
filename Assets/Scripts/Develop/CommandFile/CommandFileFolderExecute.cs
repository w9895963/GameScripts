using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace CommandFileBundle
{
    public static class FolderExecuter
    {
        const string DefaultFolder = "Scripts\\OnLoad";
        public static void ExecuteAllFiles(string localPath)
        {
            string[] files = FileF.GetAllFilePathsInFolderFromLocal(localPath, "*.txt", true);
            if (files.IsEmpty())
            {
                return;
            }
            files.ForEach((file) =>
            {
                CommandFileBundle.CommandFile.Execute(file);
            });

        }

        public static void ExecuteDefaultFolder()
        {
            ExecuteAllFiles(DefaultFolder);
        }
    }
}
