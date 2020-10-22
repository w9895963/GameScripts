using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Mods;
using static Global.Visible.VisibleUtility;
using UnityEngine;
using static Global.Mods.ModUtility;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Global {
    namespace Visible {
        public static class VisibleUtility {
            public static List<SpriteIterm> spriteLoadLibrary = new List<SpriteIterm> ();
            private const string CountDownIconPath = "Visible/倒计时";

            public static Sprite CreateSprite (Texture2D texture, float pixelsPerUnit) {
                Sprite result = null;
                var tex = texture;
                result = Sprite.Create (tex,
                    new Rect (0, 0, tex.width, tex.height),
                    new Vector2 (0.5f, 0.5f),
                    pixelsPerUnit);
                return result;
            }
            public static List<Sprite> CreateSpriteSheet (Texture2D texture, int column, int row, float pixelsPerUnit) {
                List<Sprite> result = new List<Sprite> ();
                var tex = texture;
                float width = tex.width / (float) column;
                float height = tex.height / (float) row;
                for (int ro = row - 1; ro >= 0; ro--) {
                    for (int co = 0; co < column; co++) {
                        var sprite = Sprite.Create (tex,
                            new Rect (co * width, ro * height, width, height),
                            new Vector2 (0.5f, 0.5f), pixelsPerUnit);
                        result.Add (sprite);
                    }
                }
                return result;
            }

            public static SpriteIterm FindSpritedate (Sprite sprite) {
                return spriteLoadLibrary.Find ((x) => x.spriteIdentify == sprite);
            }

            public static void AddCountDown (GameObject target, float time, Vector2 position) {
                GameObject icon = GameObject.Instantiate (Resources.Load<GameObject> (CountDownIconPath));
                icon.SetParent (target);
                icon.transform.position = position;
                var image = icon.GetComponentInChildren<Image> ();
                Sprite sprite = image.sprite;
                SpriteIterm spriteIterm = FindSpritedate (sprite);
                Timer.TimerControler timerControler = spriteIterm.ApplyAnimationTo (time, (sp) => {
                    image.sprite = sp;
                });
                Timer.WaitToCall (time, () => {
                    timerControler.Stop ();
                    icon.Destroy ();
                });

            }
            public static void AddStatusBar (GameObject target) {

            }
        }

        [System.Serializable]
        public class SpriteResourceSetting {
            public Sprite spriteIdentify;
            public SpriteType spriteType;
            public int column = 1;
            public int row = 1;
            public float speed = 1;

            public SpriteIterm ToSpriteIterm () {
                SpriteIterm sp = new SpriteIterm ();
                sp.texture = spriteIdentify.texture;
                sp.spriteIdentify = spriteIdentify;
                sp.column = column;
                sp.row = row;
                sp.spriteType = spriteType;
                sp.speed = speed;
                sp.Load ();

                return sp;
            }
        }

        [System.Serializable]
        public class SpriteIterm {
            public Texture2D texture;
            public Sprite spriteIdentify;
            public string name;
            public float pixelsPerUnit = 100;
            public SpriteType spriteType = SpriteType.Single;
            public List<Sprite> sprites = new List<Sprite> ();
            public int column = 1;
            public int row = 1;
            public float speed = 1;
            [SerializeField]
            private FileUtility.LocalFile pathObj;

            public string FullPath => pathObj.FullPath;
            public string LocalPath => pathObj.localPath;



            public SpriteIterm (string path) {
                this.pathObj = new FileUtility.LocalFile (path, true);
                this.name = System.IO.Path.GetFileNameWithoutExtension (path);
            }
            public SpriteIterm () { }
            public void WriteToDisk () {
                FileUtility.WriteAllText (System.IO.Path.ChangeExtension (FullPath, ".json"), ToJson ());
            }
            public string ToJson () {
                return JsonUtility.ToJson (this);
            }
            private void LoadSpriteFromTexture () {
                if (spriteType == SpriteType.Single) {
                    var sp = CreateSprite (texture, pixelsPerUnit);
                    sp.name = name;
                    if (spriteIdentify == null) spriteIdentify = sp;
                    sprites.Add (sp);
                } else {
                    sprites = CreateSpriteSheet (texture, column, row, pixelsPerUnit);
                    if (spriteIdentify == null) spriteIdentify = sprites[0];
                    for (int i = 0; i < sprites.Count; i++) {
                        sprites[i].name = $"{name}-{i}";
                    }
                }
            }
            public Sprite Load () {
                bool texLoadSuccess = false;
                if (texture == null) {
                    Texture2D tex = new Texture2D (2, 2);
                    texLoadSuccess = FileUtility.LoadImage (FullPath, tex);
                    if (texLoadSuccess) {
                        tex.name = name;
                        texture = tex;
                    }
                } else {
                    texLoadSuccess = true;
                }
                if (texLoadSuccess) {
                    LoadSpriteFromTexture ();
                }

                Sprite spriteIdentify = sprites.Count > 0 ? sprites[0] : null;
                if (spriteIdentify) {
                    VisibleUtility.spriteLoadLibrary.Add (this);
                }
                return spriteIdentify;
            }

            public void ApplyAnimationTo (UnityAction<Sprite> apply) {
                ApplyAnimationTo (speed, apply);
            }
            public Timer.TimerControler ApplyAnimationTo (float speed, UnityAction<Sprite> apply) {
                int index = 0;
                var timerControler = Timer.Loop (speed / sprites.Count, () => {
                    apply (sprites[index]);
                    index++;
                    if (index >= sprites.Count) {
                        index = 0;
                    }
                });
                return timerControler;
            }

        }
        public enum SpriteType { Single, Animation }

    }
}