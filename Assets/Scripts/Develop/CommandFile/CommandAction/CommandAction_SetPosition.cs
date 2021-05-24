using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SetPosition : CommandLineActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;

                float[] vs = cl.ReadParams<float>();
                if (vs.Length < 2) { return; }
                obj.SetPositionLocal(new Vector2(vs[0], vs[1]));
            }




        }
    }

}
