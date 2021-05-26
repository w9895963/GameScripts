using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SetScale : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                float[] vs = cl.ReadParams<float>();
                if (vs.Length < 2) { return; }
                obj.SetScaleLo(vs[0], vs[1]);
            }




        }
    }

}
