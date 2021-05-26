using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightIntensity : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                Light2D li = obj.GetComponent<Light2D>();
                if (cl.ParamsLength > 0)
                {
                    float v = cl.ReadParam<float>(0);
                    li.intensity = v;

                }

            }




        }
    }

}
