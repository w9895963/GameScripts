using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommandFileBundle;
using UnityEngine;



namespace SceneBundle
{
    public class SceneHolder : MonoBehaviour
    {
        public List<CommandLine> comandLines = new List<CommandLine>();

        public void Build()
        {
            comandLines.ForEach((line) =>
            {
                line.onSceneBuild?.Invoke(line);
            });
            comandLines.ForEach((line) =>
            {
                line.afterSceneBuild?.Invoke(line);
            });
        }


    }
}
