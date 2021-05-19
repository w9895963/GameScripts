using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CommandFileBundle
{


    public static class SceneManager
    {
        static string DefaultSceneName = "Default";
        static string DefaultScenePath => Application.dataPath + "/Resources/Scene/Default";

        public static void LoadDefaultScene()
        {
            SceneF.CreateScene(DefaultSceneName);
            string[] paths = Directory.GetFiles(DefaultScenePath, "*.txt");
            paths = paths.Select((x) => x.FixPath()).ToArray();
            paths.ForEach((path) =>
            {
                CommandFileBundle.CommandFile.Execute(path);
            });
        }
    }
}
