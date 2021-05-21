using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace FileBundle
{
    public static class TextureLoader
    {
        public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        public static Texture2D LoadTexture(string path, FilterMode filterMode = FilterMode.Point)
        {
            Texture2D texture = null;
            textureDict.TryGetValue(path, out texture);
            if (texture == null)
            {
                if (File.Exists(path))
                {
                    texture = new Texture2D(2, 2);
                    texture.filterMode = filterMode;
                    byte[] bytes = File.ReadAllBytes(path);
                    bool loadSuccess = ImageConversion.LoadImage(texture, bytes);
                    if (loadSuccess)
                    {
                        texture.name = Path.GetFileName(path);
                    }
                }
            }
            return texture;
        }

        public static Sprite LoadSprite(string path, float pixelsPerUnit = 20)
        {
            Texture2D tex = LoadTexture(path);
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(tex, rect, pivot, pixelsPerUnit);

        }


    }
}

