using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class TextureLoader
{
    public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
    public static Texture2D LoadFromFullPath(string path)
    {
        Texture2D texture = null;
        path.Replace(@"\", "/");
        if (File.Exists(path))
        {
            texture = new Texture2D(2, 2);
            byte[] bytes = File.ReadAllBytes(path);
            bool loadSuccess = ImageConversion.LoadImage(texture, bytes);
            if (loadSuccess)
            {
                texture.name = Path.GetFileName(path);
            }
        }
        DestroySameTexture();
        void DestroySameTexture()
        {
            if (texture == null)
            {
                bool hasLoaded = textureDict.ContainsKey(path);
                if (hasLoaded)
                {
                    GameObject.Destroy(textureDict[path]);
                }
            }
            else
            {
                bool hasLoaded = textureDict.ContainsKey(path);
                if (hasLoaded)
                {
                    GameObject.Destroy(textureDict[path]);
                    textureDict[path] = texture;
                }
                else
                {
                    textureDict.Add(path, texture);
                }
            }
        }

        return texture;
    }
    public static Texture2D LoadFromDataPath(string localPath)
    {
        string path = Path.Combine(Application.dataPath, localPath);
        path.Replace(@"\", "/");
        return LoadFromFullPath(path);
    }

}

