using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.Timer;
using UnityEngine;


namespace Global {
    namespace Animation {
        public static class AnimateUtility {
            public const string uvClampAttribute = "UVClamp";
            public const string textureAttribute = "SpriteTexture";


            public static void SetSpriteTexture (GameObject gameObject, Texture2D texture) {
                SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
                spriteRenderer.material
                    .SetTexture (textureAttribute, texture);
            }
            public static TimerControler SetTextureAnimate (GameObject gameobject, TextureAnimateProfile textureAnimation) {
                int col = textureAnimation.Column;
                int row = textureAnimation.Row;
                int texCount = col * row;
                var ta = textureAnimation;

                SetSpriteTexture (gameobject, textureAnimation.texture);
                SetAnimateSheet (gameobject, 0, col, row);

                Timer.TimerControler timerControler = null;
                if (texCount > 1) {
                    int count = 1;
                    float updateTime = ta.animationTime / (float) texCount;
                    timerControler = Timer.Loop (updateTime, () => {
                        SetSpriteTexture (gameobject, textureAnimation.texture);
                        SetAnimateSheet (gameobject, count, col, row);
                        count++;
                        count = count % texCount;
                    });

                }
                return timerControler;
            }

            public static int SetAnimateSheet (GameObject gameobject, int index, int col, int row) {
                Material material = gameobject.GetComponentInChildren<SpriteRenderer> ().material;

                Vector4 uvClamp = SheetIndexToUv (index, col, row);
                material.SetVector (uvClampAttribute, uvClamp);


                return index;
            }



            public static Vector4 SheetIndexToUv (int index, int column, int row) {
                float width = 1 / (float) column;
                float height = 1 / (float) row;
                float x = index % (column) * width;
                float y = 1 - index / column * height;
                Vector2 offset = new Vector2 (x, y);
                Vector2 scale = new Vector2 (column, row);
                Vector4 uvClamp = new Vector4 (x, x + width, y - height, y);
                return uvClamp;
            }

        }

        [System.Serializable] public class TextureAnimateProfile : IModDataContainer {
            public Texture2D texture;   
            public int Column = 1;
            public int Row = 1;
            public float animationTime = 1;

        }
    }
}