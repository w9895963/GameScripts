using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResouceLoaderBundle
{
    public static class ResouceLoader_LazyLoad
    {
        public static List<LoadFile> loadFiles = new List<LoadFile>();

        public class LoadFile
        {
            public System.Object file;
            public string path;
        }


        public static void LazyLoad<T>(string path, Action<T> action) where T : UnityEngine.Object
        {

            LoadFile loadFile = loadFiles.Find((x) => x.path == path);
            if (loadFile != null)
            {
                action((T)loadFile.file);
            }
            else
            {
                ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
                Action<AsyncOperation> actionA = (d) =>
                {
                    UnityEngine.Object file = resourceRequest.asset;
                    LoadFile lf = new LoadFile();
                    lf.path = path;
                    lf.file = file;
                    loadFiles.Add(lf);
                    action((T)file);
                };
                resourceRequest.completed += actionA;
            }

        }

    }
}
