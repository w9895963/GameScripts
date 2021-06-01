using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;




namespace EditableBundle
{
    namespace Comp
    {
        public class CompItemTexture : MonoBehaviour
        {
            public Text titleTextComponent;
            public Button buttonComponent;
            public Image imageComponent;
            public string titleText { set => titleTextComponent.text = value; get => titleTextComponent.text; }
            public void AddOnClickAction(UnityAction onClick)
            {
                buttonComponent.onClick.AddListener(onClick);
            }
            public void SetTexture(Texture texture)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                float piu = 100;
                Sprite sprite = Sprite.Create((Texture2D)texture, rect, pivot, piu);
                imageComponent.sprite = sprite;
            }

        }
    }
}
