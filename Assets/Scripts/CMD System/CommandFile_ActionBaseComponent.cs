using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CommandFileBundle
{
    public class CommandActionHolder : MonoBehaviour
    {
        public virtual int RunOrder => 0;

        public virtual void Action(CommandLine commandLine) { }

    }


}
