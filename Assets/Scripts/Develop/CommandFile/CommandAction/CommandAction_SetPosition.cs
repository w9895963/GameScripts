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
                if (obj == null)
                {
                    obj.Log();
                }
                if (obj.name.Contains("刚体"))
                {
                    obj.Log();
                }
                float[] vs = cl.ReadParams<float>();
                if (vs.Length < 2) { return; }
                obj.SetPosition(vs[0], vs[1]);
            }




        }
    }

}
