﻿using System.Collections;
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
            public static string RootModsFolderName = "Mods";
            public static string modProfileFileName = "ModSetting.json";
            public static string dataFolderName = "Datas";
            public static List<string> imageExtensions = new List<string> { ".png" };








            //* Public  Property
            public static string ModsRootFolderPath => FileUtility.GetFullPath (RootModsFolderName);




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
            public static Mod CreateMod (string modFolderName, string modName = null) {
                var modsFolder = FileUtility.GetFile (ModsRootFolderPath);
                modsFolder.CreateFolder (modFolderName);
                string name = modName.IsEmpty () ? modName : modFolderName;
                Mod mod = new Mod (modFolderName, name);
                mod.WriteModProfile ();
                return mod;
            }


            public static void LoadAllModData () {
                List<IModable> modComps = FindAllInterfaces<IModable> ();

                if (!Directory.Exists (ModsRootFolderPath)) {
                    Directory.CreateDirectory (ModsRootFolderPath);
                }
                string[] dirs = Directory.GetDirectories (ModsRootFolderPath);
                List<Mod> mods = new List<Mod> ();
                dirs.ForEach ((folderPath) => {
                    Mod mod = ReadMod (folderPath);
                    if (mod != null) {
                        mods.Add (mod);
                    }
                });
                mods.Sort ((x, y) => x.profile.loadOrder.CompareTo (y.profile.loadOrder));
                mods.ForEach ((mod) => {
                    mod.ReadModDatas ().ForEach ((modData) => {
                        if (modComps.Count > 0) {
                            IModable modableComp = modComps.Find ((x) => x.ModTitle == modData.name);
                            if (modableComp != null) {
                                modableComp.LoadModData (modData);
                            }
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
                SpriteIterm data;
                if (datafile == null) {
                    data = BuildSpriteDataFile (image.FullPath);
                } else {
                    data = datafile.ReadJson<SpriteIterm> ();
                    if (data == null) {
                        data = BuildSpriteDataFile (image.FullPath);
                    }
                }

                data.Load ();
                spriteLoadLibrary.Add (data);
                return result;
            }

            private static SpriteIterm BuildSpriteDataFile (string imageFullPath) {
                SpriteIterm data = new SpriteIterm (imageFullPath);
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
                Debug.Log (title);
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


                    if (holder as IModableSprite != null) {
                        var sprites = (holder as IModableSprite).ModableSprites;
                        List<SpriteIterm> lists = sprites.Select ((x) => FindSpritedate (x)).ToList ();
                        data.spriteDatas = lists;

                    }
                    string mainDataPath = FileUtility.GetFullPath ($"{DataFolderLocalPath}/{data.name}.json");


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
            public string name;
            public string objectJson;
            public List<SpriteIterm> spriteDatas;
            private List<Sprite> backUpSprites;
            //* Public Property
            public IModable modTarget {
                get {
                    return FindAllInterfaces<IModable> ().Find ((x) => x.ModTitle == name);
                }
            }
            public IModableSprite spriteHandler => modTarget as IModableSprite;

            public ModData (string name = null, System.Object obj = null) {
                this.name = name;
                objectJson = JsonUtility.ToJson (obj);
            }

            public void LoadObjectDataTo<T> (System.Object target) where T : class {
                if (spriteHandler != null) {
                    backUpSprites = spriteHandler.ModableSprites;
                }

                JsonUtility.FromJsonOverwrite (objectJson, target as T);

                if (spriteHandler != null) {
                    spriteHandler.ModableSprites = backUpSprites;
                }
            }

            public void LoadSprites () {
                if (spriteDatas != null) {
                    List<Sprite> spriteslist = spriteDatas.Select ((x) => x.Load ()).ToList ();
                    spriteHandler.ModableSprites = spriteslist;
                }
            }
            public string ToJson () {
                JSONNode jSONNode = JSON.Parse (JsonUtility.ToJson (this));
                jSONNode["objectJson"] = JSON.Parse (objectJson);
                return jSONNode.ToString ();
            }
            public void FromJson (string json) {
                JSONNode jSONNode = JSON.Parse (json);
                ModData modData = JsonUtility.FromJson<ModData> (json);
                name = modData.name;
                objectJson = jSONNode["objectJson"].ToString ();
                spriteDatas = modData.spriteDatas;
            }



        }




    }

}