using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SetRotate : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                float[] vs = cl.ReadParams<float>();
                if (vs.Length == 0) { return; }
                obj.SetRotate(vs[0]);
            }




        }
    }

}
