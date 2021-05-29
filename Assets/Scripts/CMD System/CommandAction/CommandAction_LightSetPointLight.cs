using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightSetPointLight : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                Light2D li = obj.GetComponent<Light2D>();
                float[] vs = cl.ReadParams<float>();
                if (vs.Length > 1)
                {
                    li.pointLightOuterRadius = vs[0];
                    li.pointLightInnerRadius = vs[1];
                }
                if (vs.Length > 3)
                {
                    li.pointLightOuterAngle = vs[2];
                    li.pointLightInnerAngle = vs[3];
                }



            }

            public override string[] ParamNames => new string[4] { "最大范围", "最小范围", "最大角度", "最小角度" };
            public override bool IsRealTimeAction => true;




        }
    }

}
