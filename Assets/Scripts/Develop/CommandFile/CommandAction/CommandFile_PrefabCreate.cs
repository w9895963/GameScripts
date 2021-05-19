using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class PrefabCreate
        {
            const string prefabFolder = "CommandPrefab";

            public static void Act(CommandLine cm)
            {
                string prefabName = cm.ReadParam<string>(0);
                string prefPath = $"{prefabFolder}/{prefabName}";
                GameObject obj = GameObjectF.CreateFromPrefab(prefPath);
                if (obj == null) { return; }
                string path = cm.Path;
                string sceneName = FileF.GetFolderName(path);

                SceneF.AddToScene(obj, sceneName);
                obj.name = Path.GetFileNameWithoutExtension(path);
                cm.GameObject = obj;
            }
        }
    }
}
