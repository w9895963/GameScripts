using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class SpriteLoaderForLight : MonoBehaviour
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
        string pa = Application.dataPath + "/" + localPath;
        byte[] bs = File.ReadAllBytes(pa);
        if (bs.Length > 0)
        {
            Sprite sprite = GetComponent<Light2D>().lightCookieSprite;
            sprite.texture.LoadImage(bs);
        }

    }
}
