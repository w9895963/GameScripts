using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightBlend : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                Light2D li = obj.GetComponent<Light2D>();
                if (cl.ParamsLength > 0)
                {
                    int v = cl.ReadParam<int>(0);
                    li.blendStyleIndex = v;

                }

            }




        }
    }

}
