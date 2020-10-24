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
            public static List<ImageIterm> ImageItermLibrary = new List<ImageIterm> ();

            public static Sprite CreateSprite (Texture2D texture, float pixelsPerUnit) {
                Sprite result = null;
                var tex = texture;
                result = Sprite.Create (tex,
                    new Rect (0, 0, tex.width, tex.height),
                    new Vector2 (0.5f, 0.5f),
                    pixelsPerUnit);
                return result;
            }


            public static ImageIterm FindImageIterm (Sprite sprite) {
                return ImageItermLibrary.Find ((x) => x.spriteIdentify == sprite);
            }
            public static ImageIterm FindImageIterm (Texture2D texture) {
                return ImageItermLibrary.Find ((x) => x.texture == texture);
            }



        }

        [System.Serializable] public class ImageIterm {
            public Texture2D texture;
            public Sprite spriteIdentify;
            public string name;
            public float pixelsPerUnit = 100;
            [SerializeField]
            private FileUtility.LocalFile pathObj;

            public string FullPath => pathObj.FullPath;
            public string LocalPath => pathObj.localPath;



            public ImageIterm (string path) {
                this.pathObj = new FileUtility.LocalFile (path, true);
                this.name = System.IO.Path.GetFileNameWithoutExtension (path);
            }
            public void WriteToDisk () {
                FileUtility.WriteAllText (System.IO.Path.ChangeExtension (FullPath, ".json"), ToJson ());
            }
            public string ToJson () {
                return JsonUtility.ToJson (this);
            }
            private Sprite LoadSpriteFromTexture () {
                if (texture != null) {
                    var sp = CreateSprite (texture, pixelsPerUnit);
                    sp.name = name;
                    if (spriteIdentify == null)
                        spriteIdentify = sp;
                }

                return spriteIdentify;
            }
            public Texture2D LoadTexture () {
                if (texture == null) {
                    Texture2D tex = new Texture2D (2, 2);
                    var texLoadSuccess = FileUtility.LoadImage (FullPath, tex);
                    if (texLoadSuccess) {
                        tex.name = name;
                        texture = tex;
                    }
                }
                return texture;
            }
            public Sprite LoadSprite () {
                LoadTexture ();

                LoadSpriteFromTexture ();


                return spriteIdentify;
            }




        }

      
    }
}