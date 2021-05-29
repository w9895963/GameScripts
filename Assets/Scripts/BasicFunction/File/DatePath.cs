using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;



namespace FileBundle
{
    public class LocalFile
    {


        const string DataFolderName = "DataFolder";
        public static string DataFolderPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        return new DirectoryInfo(Application.dataPath).Parent.FullName + "\\" + DataFolderName;
                    case RuntimePlatform.WindowsEditor:
                        return new DirectoryInfo(Application.dataPath).Parent.Parent.FullName + "\\" + DataFolderName;
                    default:
                        return Application.dataPath;
                }

            }
        }



        public readonly string localPath;
        public string FullPath => Path.Combine(DataFolderPath, localPath);
        public bool IsFile => File.Exists(FullPath);
        public bool IsDirectory => Directory.Exists(FullPath);

        private LocalFile(string localPath)
        {
            this.localPath = localPath;
        }

        public static LocalFile TryGet(string localPath)
        {
            LocalFile re = null;
            string path = Path.Combine(DataFolderPath, localPath);
            path = Path.GetFullPath(path);
            if (File.Exists(path) | Directory.Exists(path))
            {
                re = new LocalFile(localPath);
            }
            return re;
        }
    }

}
