using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
using Global.Mods;
using static Global.Function;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Global {

    namespace Mods {

        public static class ModFunc {




            //* Names and Setting
            #region All Names
            public static string RootModsFolderName = "Mods";
            public static string modProfileFileName = "ModSetting.json";
            public static string dataFolderName = "Datas";
            public static List<string> imageExtensions = new List<string> { ".png" };

            #endregion




            //* Public Fields
            public static Mod currentModBuilder;
            public static List<SpriteData> spriteLoadLibrary = new List<SpriteData> ();




            //* Public  Property
            public static string ModsRootFolderPath => FileUtility.GetFullPath (RootModsFolderName);




            //* Public Method
            #region Public Method

            public static List<Mod> GetAllMods () {
                List<Mod> result = new List<Mod> ();
                if (Directory.Exists (ModsRootFolderPath)) {
                    string[] modFolders = Directory.GetDirectories (ModsRootFolderPath);
                    modFolders.ForEach ((folder) => {
                        Mod mod = FolderToMod (folder);
                        if (mod != null) {
                            result.Add (mod);
                        }
                    });
                }
                return result;
            }
            public static Mod GetMod (string modFolderName) {
                return GetAllMods ().Find ((x) => x.ModFolderName == modFolderName);
            }
            public static Mod FolderToMod (string folderPath) {
                Mod result = null;
                string modSettingPath = FileUtility.FindFile (folderPath, modProfileFileName);
                if (File.Exists (modSettingPath)) {
                    string data = File.ReadAllText (modSettingPath);
                    ModSetting modSetting = JsonUtility.FromJson<ModSetting> (data);
                    if (modSetting != null) {
                        result = new Mod (modSetting);
                    }
                }
                return result;
            }
            public static Mod CreateMod (string modFolderName, string modName = null) {
                var modsFolder = FileUtility.GetFile (ModsRootFolderPath);
                modsFolder.CreateFolder (modFolderName);
                string name = modName.IsEmpty () ? modName : modFolderName;
                return new Mod (modFolderName, name);
            }

            public static void LoadAllModData () {
                List<IModable> modComps = Fn ().FindAllInterfaces<IModable> ();

                if (!Directory.Exists (ModsRootFolderPath)) {
                    Directory.CreateDirectory (ModsRootFolderPath);
                }
                string[] dirs = Directory.GetDirectories (ModsRootFolderPath);
                List<Mod> mods = new List<Mod> ();
                dirs.ForEach ((folderPath) => {
                    Mod mod = FolderToMod (folderPath);
                    if (mod != null) {
                        mods.Add (mod);
                    }
                });
                mods.Sort ((x, y) => x.profile.loadOrder.CompareTo (y.profile.loadOrder));
                mods.ForEach ((mod) => {
                    mod.ReadModDatas ().ForEach ((modData) => {
                        IModable modableComp = modComps.Find ((x) => x.ModTitle == modData.name);
                        if (modableComp != null) {
                            modableComp.LoadModData (modData);
                        }
                    });
                });
            }


            public static Sprite LoadImageToSprite (string fullPath) {
                Sprite result = null;
                var image = FileUtility.GetFile (fullPath);
                string dataFilename = $"{image.NameNoSuffix}.json";
                string dataPath = FileUtility.CombinePath (image.Parent.FullPath, dataFilename);
                var datafile = image.FindFileSamePlace (dataFilename);
                SpriteData data;
                if (datafile == null) {
                    data = BuildSpriteDataFile (image.FullPath);
                } else {
                    data = datafile.ReadJson<SpriteData> ();
                    if (data == null) {
                        data = BuildSpriteDataFile (image.FullPath);
                    }
                }

                data.LoadSprite ();
                spriteLoadLibrary.Add (data);
                return result;
            }

            private static SpriteData BuildSpriteDataFile (string imageFullPath) {
                SpriteData data = new SpriteData (imageFullPath);
                data.WriteToDisk ();
                return data;
            }

            public static Sprite CreateSprite (Texture2D texture, SpriteData spriteData) {
                Sprite result = null;
                var tex = texture;
                result = Sprite.Create (tex,
                    new Rect (0, 0, tex.width, tex.height),
                    new Vector2 (0.5f, 0.5f),
                    spriteData.pixelsPerUnit);
                return result;
            }

            public static SpriteData SpriteToSpritedate (Sprite sprite) {
                return spriteLoadLibrary.Find ((x) => x.spriteObject == sprite);
            }




            #endregion


        }

        public class Mod {

            public ModSetting profile;

            public Mod (string modName = "DefautMod", string modFolderName = "DefautMod", int loadOrder = 0) {
                profile = new ModSetting (modName, modFolderName, loadOrder);
                WriteModProfile ();
                ModFunc.currentModBuilder = this;
            }
            public Mod (ModSetting modSetting) {
                this.profile = modSetting;
            }


            //* Public Fields
            public string ModFolderName => profile.modFolderName;
            public string Name => profile.modName;




            #region //*Paths 
            public string RootName => ModFunc.RootModsFolderName;
            public string ModFolderPath => FileUtility.GetFullPath ($"{RootName}/{ModFolderName}");
            public string ModProfilePath => FileUtility.GetFullPath ($"{RootName}/{ModFolderName}/{ModFunc.modProfileFileName}");
            public string DataFolderLocalPath => $"{RootName}/{ModFolderName}/{ModFunc.dataFolderName}";

            #endregion




            //* Public Property
            private List<FileUtility.LocalFile> imageFiles;
            public List<FileUtility.LocalFile> ImageFiles {
                get {
                    string path = ModFolderPath;
                    if (imageFiles == null) {
                        imageFiles = FileUtility.GetAllFiles (path, ModFunc.imageExtensions.ToArray ());
                    }
                    return imageFiles;
                }
            }


            private void WriteModProfile () {
                FileUtility.WriteAllText (ModProfilePath, JsonUtility.ToJson (profile));
            }
            public void LoadAllImage () {
                ImageFiles.ForEach (((file) => {
                    ModFunc.LoadImageToSprite (file.FullPath);
                }));
            }
            public List<ModData> ReadModDatas () {
                List<ModData> result = new List<ModData> ();
                var filePaths = FileUtility.GetFiles (DataFolderLocalPath, new List<string> { ".json" });
                filePaths.ForEach ((filePath) => {
                    ModData modData = JsonUtility.FromJson<ModData> (File.ReadAllText (filePath));
                    if (modData != null) {
                        result.Add (modData);
                    }
                });
                return result;
            }
            public void WriteAllModData () {
                List<IModable> modHolders = Fn ().FindAllInterfaces<IModable> ().FindAll ((x) => x.EnableWriteModDatas);
                modHolders.ForEach ((holder) => {
                    ModData data = new ModData (holder.ModTitle, holder.ModableObjectData);
                    var sprites = (holder as IModableSprite).ModableSprites;
                    data.spriteDatas = sprites.Select ((x) => ModFunc.SpriteToSpritedate (x)).ToList ();
                    string mainDataPath = FileUtility.GetFullPath ($"{DataFolderLocalPath}/{data.name}.json");
                    FileUtility.WriteAllText (mainDataPath, JsonUtility.ToJson (data));
                });
            }


        }

        [System.Serializable]
        public class SpriteData {
            public Sprite spriteObject;
            public string FullPath => pathObj.FullPath;
            public string LocalPath => pathObj.localPath;
            [SerializeField] private FileUtility.LocalFile pathObj;
            public string name;
            public float pixelsPerUnit = 100;

            public SpriteData (string path, Sprite sprite = null) {
                this.spriteObject = sprite;
                this.pathObj = new FileUtility.LocalFile (path, true);
                this.name = System.IO.Path.GetFileNameWithoutExtension (path);
            }
            public void WriteToDisk () {
                FileUtility.WriteAllText (System.IO.Path.ChangeExtension (FullPath, ".json"), ToJson ());
            }
            public string ToJson () {
                return JsonUtility.ToJson (this);
            }

            public Sprite LoadSprite () {
                Texture2D tex = new Texture2D (2, 2);
                bool success = FileUtility.LoadImage (FullPath, tex);
                if (success) {
                    spriteObject = ModFunc.CreateSprite (tex, this);
                    spriteObject.name = name;
                }
                return spriteObject;
            }
        }

        [System.Serializable]
        public class ModSetting {
            public string modFolderName;
            public string modName;
            public int loadOrder;

            public ModSetting (string modName, string modFolderName, int loadOrder) {
                this.modName = modName;
                this.modFolderName = modFolderName;
                this.loadOrder = loadOrder;
            }
        }

        [System.Serializable]
        public class ModData {
            public string name;
            public string objectJson;
            public List<SpriteData> spriteDatas;
            private List<Sprite> backUpSprites;
            public IModable modTarget => Global.Function.Fn ().FindAllInterfaces<IModable> ()
                .Find ((x) => x.ModTitle == name);
            public IModableSprite spriteHandler => modTarget as IModableSprite;

            public ModData (string name, System.Object obj = null) {
                this.name = name;
                objectJson = JsonUtility.ToJson (obj);
            }

            public void LoadObjectDataTo<T> (System.Object target) where T : class {
                if (spriteHandler != null) {
                    backUpSprites = spriteHandler.ModableSprites;

                }

                JsonUtility.FromJsonOverwrite (objectJson, target as T);
            }

            public void LoadSprites () {
                if (spriteDatas != null) {
                    spriteHandler.ModableSprites = spriteDatas.Select ((x) => x.LoadSprite ()).ToList ();
                }
                var p = spriteHandler.ModableSprites.Select ((sprite, i) => sprite == null? backUpSprites[i] : null).ToList ();
                spriteHandler.ModableSprites = p;
            }
        }



    }

}