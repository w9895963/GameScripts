using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CommandFileBundle
{
    public class ActionBaseComponent : MonoBehaviour
    {
        public CommandLine commandLine;

        public virtual void Execute() { }
    }
}
