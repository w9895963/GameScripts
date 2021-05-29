using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CMDBundle
{
    public class CommandActionHolder : MonoBehaviour
    {
        public virtual int RunOrder => 0;

        public virtual bool IsRealTimeAction => false;
        public virtual string[] ParamNames => null;


        public virtual void Action(CommandLine commandLine) { }




    }


}
