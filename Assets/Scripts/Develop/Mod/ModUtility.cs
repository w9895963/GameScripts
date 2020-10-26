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
using System.Reflection;
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
            public static List<ModImageData> ImageItermLibrary = new List<ModImageData> ();



            //* Public Method
            #region Public Method


            public static Sprite CreateSprite (Texture2D texture, float pixelsPerUnit) {
                Sprite result = null;
                var tex = texture;
                result = Sprite.Create (tex,
                    new Rect (0, 0, tex.width, tex.height),
                    new Vector2 (0.5f, 0.5f),
                    pixelsPerUnit);
                return result;
            }


            public static ModImageData FindImageIterm (Sprite sprite) {
                return ImageItermLibrary.Find ((x) => x.sprite == sprite);
            }
            public static ModImageData FindImageIterm (Texture2D texture) {
                return ImageItermLibrary.Find ((x) => x.texture == texture);
            }

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
                    ModProjectData modSetting = JsonUtility.FromJson<ModProjectData> (data);
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


            public static void LoadAllModData () {
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

            public static void WriteAllModDataToDisk (string folderPath) {
                var modFolderName = GameObject.FindObjectOfType<ModBuilder> ().modProfile.modFolderName;

                List<IModable> modHolders = FindAllInterfaces<IModable> ().FindAll ((x) => x.EnableWriteModDatas);
                modHolders.ForEach ((holder) => {
                    ModObjectData data = new ModObjectData (holder.ModTitle, holder.ModableObjectData);


                    List<MenberInfo> spriteMembers = GetMembersFromObj (holder.ModableObjectData, typeof (Sprite));
                    var sprites = spriteMembers.Select ((x) => x.GetValue () as Sprite).ToList ();
                    data.modSprites = sprites.Select ((x) => FindImageIterm (x)).ToList ();

                    List<MenberInfo> textureMembers = GetMembersFromObj (holder.ModableObjectData, typeof (Texture2D));
                    var textures = textureMembers.Select ((x) => x.GetValue () as Texture2D).ToList ();
                    data.modTextures = textures.Select ((x) => FindImageIterm (x)).ToList ();


                    string mainDataPath = FileUtility.CombinePath (folderPath, $"{data.title}.json");

                    FileUtility.WriteAllText (mainDataPath, data.ToJson ());

                });
            }
            public static List<MenberInfo> GetMembersFromObj (System.Object sourceObject, System.Type type) {
                List<MenberInfo> result = new List<MenberInfo> ();

                var currObjs = new List<System.Object> () { sourceObject };

                int count = 0;
                while (currObjs.Count > 0 & count < 9) {
                    count++;
                    var currFiields = currObjs.SelectMany ((obj) => MenberInfo.GetAllMembers (obj)).ToList ();
                    var matchL = currFiields.Where ((x) => x.Type == type).ToList ();
                    result.AddRange (matchL);

                    List<System.Object> children = new List<System.Object> ();
                    var arrays = currFiields.Where ((x) => x.IsArrayOrList).ToList ();
                    arrays.ForEach ((ar) => {
                        IEnumerable enumerable = (IEnumerable) ar.GetValue ();
                        foreach (var item in enumerable) {
                            children.Add (item);
                        }
                    });

                    List<MenberInfo> container = currFiields.Where ((f) => f.IsContainer).ToList ();
                    children.AddRange (container.Select ((x) => x.GetValue ()));

                    currObjs = children;
                }
                return result;
            }
            public static List<T> GetMemberValuesFromObj<T> (System.Object sourceObject) where T : class {
                List<MenberInfo> result = GetMembersFromObj (sourceObject, typeof (T));
                return result.Select ((x) => x.GetValue<T> ()).ToList ();
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


            public static Sprite LoadImage (string fullPath) {
                Sprite result = null;
                var image = FileUtility.GetFile (fullPath);
                string dataFilename = $"{image.NameNoSuffix}.json";
                string dataPath = FileUtility.CombinePath (image.Parent.FullPath, dataFilename);
                var datafile = image.FindFileSamePlace (dataFilename);
                ModImageData data;
                if (datafile == null) {
                    data = BuildModImageDataFile (image.FullPath);
                } else {
                    data = datafile.ReadJson<ModImageData> ();
                    if (data == null) {
                        data = BuildModImageDataFile (image.FullPath);
                    }
                }

                data.Load ();
                ImageItermLibrary.Add (data);
                return result;
            }

            private static ModImageData BuildModImageDataFile (string imageFullPath) {
                ModImageData data = ModImageData.CreateDefault (imageFullPath);
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




            #endregion


        }

        public class Mod {

            public ModProjectData profile;

            public Mod (string modName = "DefautMod", string modFolderName = "DefautMod", int loadOrder = 0) {
                profile = new ModProjectData (modName, modFolderName, loadOrder);
            }
            public Mod (ModProjectData modSetting) {
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
                    ModUtility.LoadImage (file.FullPath);
                }));
            }
            public List<ModObjectData> ReadModDatas () {
                List<ModObjectData> result = new List<ModObjectData> ();
                var filePaths = FileUtility.GetFiles (DataFolderLocalPath, new List<string> { ".json" });
                filePaths.ForEach ((filePath) => {

                    string json = File.ReadAllText (filePath);
                    // ModData modData = JsonUtility.FromJson<ModData> (json);
                    ModObjectData modData = new ModObjectData ();

                    modData.FromJson (json);

                    if (modData != null) {
                        result.Add (modData);
                    }
                });
                return result;
            }
            public void WriteAllModData () {
                WriteAllModDataToDisk (FileUtility.GetFullPath (DataFolderLocalPath));
            }


        }

        [System.Serializable]
        public class ModProjectData {
            public string modFolderName;
            public string modName;
            public int loadOrder;

            public ModProjectData (string modName, string modFolderName, int loadOrder) {
                this.modName = modName;
                this.modFolderName = modFolderName;
                this.loadOrder = loadOrder;
            }
        }

        [System.Serializable]
        public class ModObjectData {
            public string title;
            public string objectJson;

            public List<ModImageData> modSprites = new List<ModImageData> ();
            public List<ModImageData> modTextures = new List<ModImageData> ();
            private List<Sprite> backUpSprites;
            private List<Texture2D> backUpTexture;
            //* Public Property
            public IModable modTarget {
                get {
                    return FindAllInterfaces<IModable> ().Find ((x) => x.ModTitle == title);
                }
            }

            public ModObjectData (string name = null, System.Object obj = null) {
                this.title = name;
                objectJson = JsonUtility.ToJson (obj);
            }

            public void LoadObjectDataTo<T> (System.Object target) where T : class {
                backUpSprites = GetMemberValuesFromObj<Sprite> (target);
                backUpTexture = GetMemberValuesFromObj<Texture2D> (target);
                List<MenberInfo> sprites = GetMembersFromObj (target, typeof (Sprite));
                List<MenberInfo> textures = GetMembersFromObj (target, typeof (Texture2D));


                JsonUtility.FromJsonOverwrite (objectJson, target as T);

                if (modSprites.Count > 0) {
                    List<Sprite> spriteLoads = modSprites.Select ((x) => x.Load ().sprite).ToList ();
                    for (int i = 0; i < spriteLoads.Count; i++) {
                        if (spriteLoads[i] != null) {
                            sprites[i].SetValue (spriteLoads[i]);
                        }
                    }
                }
                if (modTextures.Count > 0) {
                    List<Texture2D> textureLoads = modTextures.Select ((x) => x.Load ().texture).ToList ();
                    for (int i = 0; i < textureLoads.Count; i++) {
                        if (textureLoads[i] != null) {
                            textures[i].SetValue (textureLoads[i]);
                        }
                    }
                }




                for (int i = 0; i < sprites.Count; i++) {
                    if (sprites[i].GetValue<Sprite> () == null) {
                        sprites[i].SetValue (backUpSprites[i]);
                    }
                }

                for (int i = 0; i < textures.Count; i++) {
                    if (textures[i].GetValue<Texture2D> () == null) {
                        textures[i].SetValue (backUpTexture[i]);
                    }
                }
            }


            public string ToJson () {
                JSONNode jSONNode = JSON.Parse (JsonUtility.ToJson (this));
                jSONNode["objectJson"] = JSON.Parse (objectJson);
                return jSONNode.ToString ();
            }
            public void FromJson (string json) {
                JSONNode jSONNode = JSON.Parse (json);
                ModObjectData modData = JsonUtility.FromJson<ModObjectData> (json);
                title = modData.title;
                objectJson = jSONNode["objectJson"].ToString ();
                modTextures = modData.modTextures;
            }


        }

        [System.Serializable] public class ModImageData {
            public Texture2D texture;
            public Sprite sprite;
            [SerializeField]
            private FileUtility.LocalFile pathObj;

            public string FullPath => pathObj.FullPath;
            public string LocalPath => pathObj.localPath;



            public static ModImageData CreateDefault (string path) {
                ModImageData data = new ModImageData ();
                data.pathObj = new FileUtility.LocalFile (path, true);
                return data;
            }

            public void WriteToDisk () {
                FileUtility.WriteAllText (System.IO.Path.ChangeExtension (FullPath, ".json"), JsonUtility.ToJson (this));
            }


            private Texture2D LoadTexture () {
                Texture2D tex = new Texture2D (2, 2);
                var texLoadSuccess = FileUtility.LoadImage (FullPath, tex);
                if (texLoadSuccess) {
                    tex.name = pathObj.NameNoSuffix;
                    texture = tex;
                }
                return texture;
            }
            private void LoadSprite () {
                sprite = CreateSprite (texture, 100);
                sprite.name = pathObj.NameNoSuffix;
            }

            public ModImageData Load () {
                if (texture == null) {
                    LoadTexture ();
                }

                if (sprite == null & texture != null) {
                    LoadSprite ();
                }

                return this;

            }



        }

        public class MenberInfo {
            public System.Object sourceObj;
            public FieldInfo fieldInfo;
            public System.Type Type => fieldInfo.FieldType;
            public bool IsContainer => HasInterface (typeof (IModDataContainer));
            public bool IsArrayOrList => fieldInfo.FieldType.IsArray | fieldInfo.FieldType.IsList ();
            public bool HasInterface (System.Type type) {
                return fieldInfo.FieldType.GetInterfaces ().Contains (type);
            }
            public object GetValue () {
                return fieldInfo.GetValue (sourceObj);
            }
            public void SetValue (object value) {
                fieldInfo.SetValue (sourceObj, value);
            }
            public T GetValue<T> () where T : class {
                return fieldInfo.GetValue (sourceObj) as T;
            }


            public static List<MenberInfo> GetAllMembers (System.Object obj) {
                List<MenberInfo> lists = obj.GetType ().GetFields ().Select ((f) => {
                    MenberInfo result = new MenberInfo ();
                    result.sourceObj = obj;
                    result.fieldInfo = f;
                    return result;
                }).ToList ();
                return lists;
            }


        }



    }

}