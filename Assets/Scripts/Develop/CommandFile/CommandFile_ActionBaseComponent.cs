using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CommandFileBundle
{
    public class CommandLineActionHolder : MonoBehaviour
    {
        public SceneBundle.SceneBuildEvent onSceneEvent = SceneBundle.SceneBuildEvent.OnBuild;

        public virtual void Action(CommandLine commandLine) { }
       /*  public virtual void Action(CommandLine commandLine) { }
        public virtual void AfterSceneBuild(CommandLine commandLine) { } */
    }


}
