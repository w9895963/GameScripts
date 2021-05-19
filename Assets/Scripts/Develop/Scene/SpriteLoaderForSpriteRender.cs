using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteLoaderForSpriteRender : MonoBehaviour
{
    public string localPath;

    private void Update()
    {
        float key = Keyboard.current.f2Key.ReadValue();
        if (key > 0)
        {
            Load();
        }
    }



    private void Load()
    {
        Texture2D texture2D =FileF.LoadTexture(localPath);
        if (texture2D != null)
        {
            texture2D.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            float pixelsPerUnit = 100;
            Sprite sprite = Sprite.Create(texture2D, rect, pivot, pixelsPerUnit);
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
