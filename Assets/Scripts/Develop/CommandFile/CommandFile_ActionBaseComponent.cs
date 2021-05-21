using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CommandFileBundle
{
    public class CommandLineActionHolder : MonoBehaviour
    {

        public virtual void OnSceneBuild(CommandLine commandLine) { }
        public virtual void AfterSceneBuild(CommandLine commandLine) { }
    }


}
