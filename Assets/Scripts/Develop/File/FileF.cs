using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileF
{
    public static Texture2D LoadTexture(string path, FilterMode filterMode = FilterMode.Point)
    {
        return FileBundle.TextureLoader.LoadTexture(path, filterMode);
    }
    public static Sprite LoadSprite(string path, float pixelsPerUnit = 20)
    {
        return FileBundle.TextureLoader.LoadSprite(path, pixelsPerUnit);
    }

    public static string GetFullPath(string localPath)
    {
        return DataFile.TryGet(localPath)?.FullPath;
    }



    public static string[] GetAllFilePathsInFolder(string path, string match = "*", bool includeChildren = false)
    {
        if (!Directory.Exists(path)) { return null; }
        List<string> paths = Directory.GetFiles(path, match).ToList();
        if (includeChildren)
        {
            List<string> folders = Directory.GetDirectories(path).ToList();
            while (folders.Count > 0)
            {
                List<string> files = folders.SelectMany((x) => Directory.GetFiles(x, match)).ToList();
                folders = folders.SelectMany((x) => Directory.GetDirectories(x)).ToList();
                paths.AddRange(files);
            }
        }
        return paths.ToArray();
    }
    public static string[] GetAllFilesInFolderFromLocal(string localPath, string match = "*", bool includeChildren = false)
    {
        return GetAllFilePathsInFolder(GetFullPath(localPath), match, includeChildren);
    }



    public static string GetFolderName(string path)
    {
        DirectoryInfo file = new DirectoryInfo(path);
        return file.Parent.Name;
    }
    public static string GetFilePathInSameFolder(string path, string fileName)
    {

        return Path.GetDirectoryName(path) + "\\" + fileName;
    }



}
