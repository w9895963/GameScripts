using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_CreateObject : ActionBaseComponent
        {
            public override void Execute()
            {
                commandLine.GameObject = gameObject;
            }


        }
    }

}
