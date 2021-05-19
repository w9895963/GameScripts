using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataFile
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

    private DataFile(string localPath)
    {
        this.localPath = localPath;
    }

    public static DataFile TryGet(string localPath)
    {
        DataFile re = null;
        string path = Path.Combine(DataFolderPath, localPath);
        if (File.Exists(path) | Directory.Exists(path))
        {
            re = new DataFile(localPath);
        }
        return re;
    }
}
