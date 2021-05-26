using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace CMDBundle
{
    public static class FileReader
    {
        const string ScriptFolder = "Scripts";
        public static void AddAllCommandFilesInFolderToScene(string localPath, string sceneName)
        {
            string[] paths = FileF.GetAllFilesFromLocal(localPath, "*.txt", true);
            if (paths.IsEmpty())
            {
                return;
            }
            paths.ForEach((path) =>
            {
                CommandFile commandFile = CMDBundle.CommandFile.TryGetCommandFile(path);
                commandFile.sceneName = sceneName;
                if (commandFile == null) { return; }
                SceneBundle.SceneHolder sceneHolder = SceneF.FindOrCreateScene(sceneName);
                sceneHolder.comandFiles.Add(commandFile);
            });

        }

        public static void ReadAllScripts()
        {
            CommandFile.AllFiles = new List<CommandFile>();
            string[] folderPaths = FileF.GetAllFoldersFromLocal(ScriptFolder);
            folderPaths.ForEach((folder) =>
            {
                AddAllCommandFilesInFolderToScene(folder, Path.GetFileName(folder));
            });
        }

    }
}
