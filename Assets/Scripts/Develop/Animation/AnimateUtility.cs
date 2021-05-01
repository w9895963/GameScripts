using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.Timer;
using UnityEngine;
using UnityEngine.Events;

namespace Global
{
    namespace Animate
    {
        public static class AnimateUtility
        {
            public const string uvClampAttribute = "UVClamp";
            public const string textureAttribute = "SpriteTexture";

            public static void ChangeAnimation(GameObject gameObject, GameObject animation, float facing)
            {
                if (animation != null)
                {
                    GameObject animationHolder = gameObject.GetComponentInChildren<CharacterAnimatorLo>().gameObject;
                    animationHolder.FindAllChild().Destroy();
                    GameObject aniObj = animationHolder.CreateChild(animation);
                    Vector3 localScale = aniObj.transform.localScale;
                    aniObj.transform.localScale = new Vector3(localScale.x * facing, localScale.y, localScale.z);
                }
            }

            public static TimerControler AnimateInt(int start, int end, float duration, UnityAction<int> callcack, AnimationCurve timeCurve = null)
            {
                int currIndex = start;
                Timer.TimerControler timerControler = null;
                timeCurve = timeCurve != null ? timeCurve : Curve.ZeroOne;

                callcack(start);

                timerControler = Timer.DynimicLoop(DurationCalc, Callback);

                return timerControler;



                void Callback()
                {
                    currIndex++;
                    if (currIndex > end)
                    {
                        currIndex = 0;
                    }
                    callcack(currIndex);
                }

                float DurationCalc()
                {
                    float beginTime = timerControler.beginTime;
                    float currentTime = timerControler.CurrentTime;
                    float max = ((float)(end - start + 1)).ClampMin(1);

                    float scale = timeCurve.Evaluate((currIndex - start + 1) / max) - timeCurve.Evaluate((currIndex - start) / max);


                    return duration * scale;
                }
            }
            public static Material CloneMaterial(GameObject gameObject)
            {
                SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
                Material material = render.material;
                render.material = GameObject.Instantiate(material);
                return render.material;
            }

            public static void SetSpriteMaterialTexture(GameObject gameObject, Texture2D texture)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.material
                    .SetTexture(textureAttribute, texture);
            }
            public static TimerControler SetTextureAnimate(GameObject gameobject, TextureAnimateProfile textureAnimation)
            {
                int col = textureAnimation.Column;
                int row = textureAnimation.Row;
                int texCount = col * row;
                var ta = textureAnimation;

                SetSpriteMaterialTexture(gameobject, textureAnimation.texture);
                SetAnimateUV(gameobject, 0, col, row);

                Timer.TimerControler timerControler = null;
                if (texCount > 1)
                {
                    int count = 1;
                    float updateTime = ta.animationTime / (float)texCount;
                    timerControler = Timer.Loop(updateTime, () =>
                    {
                        SetSpriteMaterialTexture(gameobject, textureAnimation.texture);
                        SetAnimateUV(gameobject, count, col, row);
                        count++;
                        count = count % texCount;
                    });

                }
                return timerControler;
            }


            private static int SetAnimateUV(GameObject gameobject, int index, int col, int row)
            {
                Material material = gameobject.GetComponentInChildren<SpriteRenderer>().material;

                Vector4 uvClamp = SheetIndexToUv(index, col, row);
                material.SetVector(uvClampAttribute, uvClamp);


                return index;
            }

            public static Vector4 SheetIndexToUv(int index, int column, int row)
            {
                float width = 1 / (float)column;
                float height = 1 / (float)row;
                float x = index % (column) * width;
                float y = 1 - index / column * height;
                Vector2 offset = new Vector2(x, y);
                Vector2 scale = new Vector2(column, row);
                Vector4 uvClamp = new Vector4(x, x + width, y - height, y);
                return uvClamp;
            }

        }

        [System.Serializable]
        public class TextureAnimateProfile : IModDataContainer
        {
            public Texture2D texture;
            public int Column = 1;
            public int Row = 1;
            public float animationTime = 1;

        }
    }
}