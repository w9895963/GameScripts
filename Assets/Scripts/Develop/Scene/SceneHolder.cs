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

            comandLines.Where((x) => x.sceneBuildEvent == SceneBuildEvent.OnBuild).ForEach((line) =>
            {
                line.Execute();
            });
            comandLines.Where((x) => x.sceneBuildEvent == SceneBuildEvent.AfterBuild).ForEach((line) =>
            {
                line.Execute();
            });

        }


    }


    public enum SceneBuildEvent
    {
        OnBuild,
        AfterBuild
    }
}
