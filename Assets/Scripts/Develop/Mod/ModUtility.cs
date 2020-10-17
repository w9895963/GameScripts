using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.UI;
using static Global.Funtion;


namespace Global {
    public class ModUtility {

        #region //*All Names
        public static string ModsFolderName = "Mods";
        public static string modProfileFileName = "ModSetting.json";
        public static string dataFolderName = "Datas";
        public static string libraryFolderName = "Library";
        public static string imageFolderName = "Images";
        public static string defaultTextureName = "ModLoadTexture";
        private static string defaultSpriteName = "ModLoadSprite";
        #endregion


        public static Mod currentModBuilder;
        public static List<SpriteData> spriteLoadLibrary = new List<SpriteData> ();

        //* Private Fields
        private static List<string> imageExtensions = new List<string> { ".png" };
        private static List<IModable> modableComponentlist;




        //* Public  Property

        public static string ModsRootFolderPath => FileUtility.GetFullPath (ModsFolderName);

        public static List<IModable> ModableComponents {
            get {
                if (modableComponentlist == null) {
                    modableComponentlist = FindAllInterfaces<IModable> ();
                }
                return modableComponentlist;
            }
        }




        //* Public Method
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
                mod.ReadModDatas ().ForEach ((dataHolder) => {
                    IModable modable = ModableComponents.Find ((x) => x.ModDataName == dataHolder.name);
                    if (modable != null) {
                        modable.LoadModData (dataHolder.data);
                    }
                });
            });
        }

        private static List<T> FindAllInterfaces<T> () where T : class {
            InterfaceHolder[] holders = GameObject.FindObjectsOfType<InterfaceHolder> ();
            return holders.ToList ().Select ((x) => x as T).Distinct ().ToList ();
        }
        public static void RenewModableComponentsList () {
            modableComponentlist = FindAllInterfaces<IModable> ();
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
        public static Sprite LoadSprite (SpriteData spritedata) {
            Sprite sprite = null;
            if (spritedata != null) {
                Texture2D tex = new Texture2D (2, 2);
                bool sucss = FileUtility.LoadImage (spritedata.FullPath, tex);
                if (sucss) {
                    sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
                    spritedata.spriteObject = sprite;
                    sprite.name = Path.GetFileNameWithoutExtension (spritedata.FullPath);
                    spriteLoadLibrary.Add (spritedata);
                }
            }
            return sprite;

        }
        public static void LoadSpriteTo (string json, ref Sprite spriteHolder) {
            Sprite sprite = LoadSprite (JsonUtility.FromJson<SpriteData> (json));
            if (sprite) {
                spriteHolder = sprite;
            }
        }


        public static SpriteData SpriteToSpritedate (Sprite sprite) {
            return spriteLoadLibrary.Find ((x) => x.spriteObject == sprite);
        }

        public static string ToStoreData (params System.Object[] objets) {
            Debug.Log (objets.Length);
            StoreData st = new StoreData ();
            st.data = objets.ToList ().Select ((x) => JsonUtility.ToJson (x)).ToList ();
            return JsonUtility.ToJson (st);
        }
        public static List<string> FomeStoreData (string json) {
            StoreData storeData = JsonUtility.FromJson<StoreData> (json);
            Debug.Log (storeData.data.Count);
            return storeData.data;
        }



        //* Class Definition
        public class Mod {

            public ModSetting profile;

            public Mod (string modName = "DefautMod", string modFolderName = "DefautMod", int loadOrder = 0) {
                profile = new ModSetting (modName, modFolderName, loadOrder);
                WriteModProfile ();
                currentModBuilder = this;
            }
            public Mod (ModSetting modSetting) {
                this.profile = modSetting;
                currentModBuilder = this;
            }


            //* Public Fields
            public string ModFolderName => profile.modFolderName;
            public string Name => profile.modName;

            #region //*Paths 
            public string ModFolderPath => FileUtility.GetFullPath ($"{ModsFolderName}/{ModFolderName}");
            public string ModProfilePath => FileUtility.GetFullPath ($"{ModsFolderName}/{ModFolderName}/{modProfileFileName}");
            public string LibraryPath => FileUtility.GetFullPath ($"{ModsFolderName}/{ModFolderName}/{libraryFolderName}");
            public string DataFolderPath => FileUtility.GetFullPath ($"{ModsFolderName}/{ModFolderName}/{dataFolderName}");
            public string DataFolderLocalPath => $"{ModsFolderName}/{ModFolderName}/{dataFolderName}";

            #endregion

            //* Public  Property
            private List<FileUtility.LocalFile> imageFiles;
            public List<FileUtility.LocalFile> ImageFiles {
                get {
                    string path = ModFolderPath;
                    if (imageFiles == null) {
                        imageFiles = FileUtility.GetAllFiles (path, imageExtensions.ToArray ());
                    }
                    return imageFiles;
                }
            }


            private void WriteModProfile () {
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
                    ModData modData = JsonUtility.FromJson<ModData> (File.ReadAllText (filePath));
                    if (modData != null) {
                        result.Add (modData);
                    }
                });
                return result;
            }
            public void WriteAllModData () {
                RenewModableComponentsList ();
                List<IModable> modHolders = modableComponentlist.FindAll ((x) => x.EnableWriteModDatas);
                modHolders.ForEach ((holder) => {
                    ModData data = new ModData (holder.ModDataName, holder.ModData);
                    string fullPath = FileUtility.GetFullPath ($"{DataFolderLocalPath}/{holder.ModDataName}.json");
                    FileUtility.WriteAllText (fullPath, JsonUtility.ToJson (data));
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

            public void LoadSprite () {
                Texture2D tex = new Texture2D (2, 2);
                bool success = FileUtility.LoadImage (FullPath, tex);
                if (success) {
                    spriteObject = CreateSprite (tex, this);
                    spriteObject.name = name;
                }
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
            public string data;

            public ModData (string name, string data) {
                this.name = name;
                this.data = data;
            }
        }

        [System.Serializable]
        public class ModImage {
            public string path;

            public Sprite Mod (Sprite unModSprite) {
                Sprite result = unModSprite;
                Texture2D tex = new Texture2D (2, 2);
                bool suc = FileUtility.LoadImage (path, tex);
                if (suc) {
                    result = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), Vector2.one / 2f);
                    Debug.Log (result.GetInstanceID ());
                }
                return result;
            }
        }

        [System.Serializable]
        public class StoreData {
            public List<string> data = new List<string> ();

        }
    }

}