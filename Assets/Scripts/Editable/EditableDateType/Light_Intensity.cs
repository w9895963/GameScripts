using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System.Linq;
using EditableBundle.Comp;
using System;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;
using static EditableBundle.ShareDate;

namespace EditableBundle
{


    namespace DateType
    {
        public class Light_Intensity : EditDate
        {

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "基础灯光属性",
                paramNames = new[] { "亮度" },
            };








            public override Type[] DateTypes => new[] { typeof(float) };
            public override System.Object[] GetDate()
            {
                var com = gameObject.GetComponentInChildren<Light2D>();
                if (com == null) return null;
                System.Object[] re = new System.Object[] { com.intensity };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                var com = gameObject.GetComponentInChildren<Light2D>();
                if (com == null) return;

                var intensity = dates[0] as float?;
                if (intensity != null)
                {
                    com.intensity = intensity.Value;
                }


            }


        }
    }
}
