using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
using Global.Mods;
using static Global.Function;
using System;
using SimpleJSON;
using static Global.Mods.ModUtility;
using Global.Visible;
using static Global.Visible.VisibleUtility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Global {

    namespace Mods {

        public static class ModUtility {


            //* Names and Setting
            public static string BuildinModsFolderName = "Buildin";
            public static string RootModsFolderName = "Mods";
            public static string modProfileFileName = "ModSetting.json";
            public static string dataFolderName = "Datas";
            public static List<string> imageExtensions = new List<string> { ".png" };




            //* Public  Property
            public static string ModsRootFolderPath => FileUtility.GetFullPath (RootModsFolderName);
            public static string BuildinFolderPath => FileUtility.GetFullPath (BuildinModsFolderName);




            //* Public Method
            #region Public Method

            public static List<Mod> ReadAllMods () {
                List<Mod> result = new List<Mod> ();
                if (Directory.Exists (ModsRootFolderPath)) {
                    string[] modFolders = Directory.GetDirectories (ModsRootFolderPath);
                    modFolders.ForEach ((folder) => {
                        Mod mod = ReadMod (folder);
                        if (mod != null) {
                            result.Add (mod);
                        }
                    });
                }
                return result;
            }
            public static Mod FindMod (string modFolderName) {
                return ReadAllMods ().Find ((x) => x.ModFolderName == modFolderName);
            }
            public static Mod ReadMod (string folderPath) {
                Mod result = null;
                string modSettingPath = FileUtility.FindFile (folderPath, modProfileFileName);
                if (File.Exists (modSettingPath)) {
                    string data = File.ReadAllText (modSettingPath);
                    ModProfile modSetting = JsonUtility.FromJson<ModProfile> (data);
                    if (modSetting != null) {
                        result = new Mod (modSetting);
                    }
                }
                return result;
            }
            public static Mod CreateMod (string modFolderName, string modName = null, int loadOrder = 0) {
                var modsFolder = FileUtility.GetFile (ModsRootFolderPath);
                modsFolder.CreateFolder (modFolderName);
                string name = modName.IsEmpty () ? modName : modFolderName;
                Mod mod = new Mod (modFolderName, name);
                mod.profile.loadOrder = loadOrder;
                mod.WriteModProfile ();
                return mod;
            }


            public static void LoadModProcess () {
                List<IModable> modComps = FindAllInterfaces<IModable> ();


                List<Mod> allMods = new List<Mod> ();
                allMods.AddRange (GetMods (BuildinFolderPath));
                allMods.AddRange (GetMods (ModsRootFolderPath));

                var allModDatas = allMods.SelectMany ((mod) =>
                    mod.ReadModDatas ()).ToList ();

                modComps.ForEach ((comp) => {
                    var datas = allModDatas.FindAll ((data) => data.title == comp.ModTitle);
                    datas.ForEach ((data) => {
                        comp.LoadModData (data);
                    });
                });

            }

            private static List<Mod> GetMods (string modsFolderPath) {
                if (Directory.Exists (modsFolderPath)) {
                    string[] dirs = Directory.GetDirectories (modsFolderPath);
                    List<Mod> mods = new List<Mod> ();
                    dirs.ForEach ((folderPath) => {
                        Mod mod = ReadMod (folderPath);
                        if (mod != null) {
                            mods.Add (mod);
                        }
                    });
                    return mods;
                }
                return new List<Mod> (0);
            }


            public static Sprite LoadImageToSprite (string fullPath) {
                Sprite result = null;
                var image = FileUtility.GetFile (fullPath);
                string dataFilename = $"{image.NameNoSuffix}.json";
                string dataPath = FileUtility.CombinePath (image.Parent.FullPath, dataFilename);
                var datafile = image.FindFileSamePlace (dataFilename);
                ImageIterm data;
                if (datafile == null) {
                    data = BuildSpriteDataFile (image.FullPath);
                } else {
                    data = datafile.ReadJson<ImageIterm> ();
                    if (data == null) {
                        data = BuildSpriteDataFile (image.FullPath);
                    }
                }

                data.LoadSprite ();
                ImageItermLibrary.Add (data);
                return result;
            }

            private static ImageIterm BuildSpriteDataFile (string imageFullPath) {
                ImageIterm data = new ImageIterm (imageFullPath);
                data.WriteToDisk ();
                return data;
            }




            public static string GenerateTitle (MonoBehaviour component) {
                string title = component.name;
                List<GameObject> lists = component.gameObject.GetParents ();
                lists.ForEach ((obj) => {
                    title = title.Insert (0, $"{obj.name}, ");
                });
                title = title.Insert (0, $"{SceneManager.GetActiveScene().name}, ");
                return title;
            }

            public static string ToJson (ModData data, List < (string key, string json) > maps = null) {
                string re = JsonUtility.ToJson (data);
                SimpleJSON.JSONNode jSONNode = SimpleJSON.JSON.Parse (re);
                if (maps != null) {
                    maps.ForEach ((x) => {
                        jSONNode[x.key] = x.json;
                    });
                }
                return jSONNode.ToString ();
            }




            #endregion


        }

        public class Mod {

            public ModProfile profile;

            public Mod (string modName = "DefautMod", string modFolderName = "DefautMod", int loadOrder = 0) {
                profile = new ModProfile (modName, modFolderName, loadOrder);
            }
            public Mod (ModProfile modSetting) {
                this.profile = modSetting;
            }


            //* Public Fields
            public string ModFolderName => profile.modFolderName;
            public string Name => profile.modName;




            #region //*Paths 
            public string RootName => ModUtility.RootModsFolderName;
            public string ModFolderPath => FileUtility.GetFullPath ($"{RootName}/{ModFolderName}");
            public string ModProfilePath => FileUtility.GetFullPath ($"{RootName}/{ModFolderName}/{ModUtility.modProfileFileName}");
            public string DataFolderLocalPath => $"{RootName}/{ModFolderName}/{ModUtility.dataFolderName}";

            #endregion




            //* Public Property
            private List<FileUtility.LocalFile> imageFiles;
            public List<FileUtility.LocalFile> ImageFiles {
                get {
                    string path = ModFolderPath;
                    if (imageFiles == null) {
                        imageFiles = FileUtility.GetAllFiles (path, ModUtility.imageExtensions.ToArray ());
                    }
                    return imageFiles;
                }
            }


            public void WriteModProfile () {
                FileUtility.WriteAllText (ModProfilePath, JsonUtility.ToJson (profile));
            }
            public void LoadAllImage () {
                ImageFiles.ForEach (((file) => {
                    ModUtility.LoadImageToSprite (file.FullPath);
                }));
            }
            public List<ModData> ReadModDatas () {
                List<ModData> result = new List<ModData> ();
                var filePaths = FileUtility.GetFiles (DataFolderLocalPath, new List<string> { ".json" });
                filePaths.ForEach ((filePath) => {

                    string json = File.ReadAllText (filePath);
                    // ModData modData = JsonUtility.FromJson<ModData> (json);
                    ModData modData = new ModData ();

                    modData.FromJson (json);

                    if (modData != null) {
                        result.Add (modData);
                    }
                });
                return result;
            }
            public void WriteAllModData () {
                List<IModable> modHolders = FindAllInterfaces<IModable> ().FindAll ((x) => x.EnableWriteModDatas);
                modHolders.ForEach ((holder) => {
                    ModData data = new ModData (holder.ModTitle, holder.ModableObjectData);

                    data.ImageIterms = new List<ImageIterm> ();
                    if (holder as IModableSprite != null) {
                        var sprites = (holder as IModableSprite).ModableSprites;
                        List<ImageIterm> lists = sprites.Select ((x) => FindImageIterm (x)).ToList ();
                        data.ImageIterms.AddRange (lists);

                    }
                    if (holder as IModableTexture != null) {
                        var textureList = (holder as IModableTexture).ModableTexture;
                        List<ImageIterm> lists = textureList.Select ((x) => FindImageIterm (x)).ToList ();
                        data.ImageIterms.AddRange (lists);
                        data.ImageIterms = data.ImageIterms.Distinct ().ToList ();

                    }
                    string mainDataPath = FileUtility.GetFullPath ($"{DataFolderLocalPath}/{data.title}.json");


                    FileUtility.WriteAllText (mainDataPath, data.ToJson ());

                });
            }


        }

        [System.Serializable]
        public class ModProfile {
            public string modFolderName;
            public string modName;
            public int loadOrder;

            public ModProfile (string modName, string modFolderName, int loadOrder) {
                this.modName = modName;
                this.modFolderName = modFolderName;
                this.loadOrder = loadOrder;
            }
        }

        [System.Serializable]
        public class ModData {
            public string title;
            public string objectJson;
            public List<ImageIterm> ImageIterms;
            private List<Sprite> backUpSprites;
            private List<Texture2D> backUpTexture;
            //* Public Property
            public IModable modTarget {
                get {
                    return FindAllInterfaces<IModable> ().Find ((x) => x.ModTitle == title);
                }
            }
            public IModableSprite spriteHandler => modTarget as IModableSprite;
            public IModableTexture textureHandler => modTarget as IModableTexture;

            public ModData (string name = null, System.Object obj = null) {
                this.title = name;
                objectJson = JsonUtility.ToJson (obj);
            }

            public void LoadObjectDataTo<T> (System.Object target) where T : class {
                if (spriteHandler != null) {
                    backUpSprites = spriteHandler.ModableSprites;
                }
                if (textureHandler != null) {
                    backUpTexture = textureHandler.ModableTexture;
                }

                JsonUtility.FromJsonOverwrite (objectJson, target as T);

                if (spriteHandler != null) {
                    spriteHandler.ModableSprites = backUpSprites;
                }
                if (textureHandler != null) {
                    textureHandler.ModableTexture = backUpTexture;
                }
            }

            public void LoadSprites () {
                if (ImageIterms != null) {
                    List<Sprite> spriteslist = ImageIterms.Select ((x) => x.LoadSprite ()).ToList ();
                    spriteHandler.ModableSprites = spriteslist;
                }
            }
            public List<Texture2D> LoadTexture () {
                List<Texture2D> result = null;
                if (ImageIterms != null) {
                    List<Texture2D> list = ImageIterms.Select ((x) => x.LoadTexture ()).ToList ();
                    result = list;
                }
                return result;
            }
            public string ToJson () {
                JSONNode jSONNode = JSON.Parse (JsonUtility.ToJson (this));
                jSONNode["objectJson"] = JSON.Parse (objectJson);
                return jSONNode.ToString ();
            }
            public void FromJson (string json) {
                JSONNode jSONNode = JSON.Parse (json);
                ModData modData = JsonUtility.FromJson<ModData> (json);
                title = modData.title;
                objectJson = jSONNode["objectJson"].ToString ();
                ImageIterms = modData.ImageIterms;
            }



        }




    }

}