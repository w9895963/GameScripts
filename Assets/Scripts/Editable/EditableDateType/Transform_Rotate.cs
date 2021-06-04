using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System;

namespace EditableBundle
{


    namespace DateType
    {
        public class Transform_Rotate : EditDate
        {
            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "旋转",
                paramNames = new[] { "角度" },
            };

            public override Type[] DateTypes => new[] { typeof(float) };
            public override System.Object[] GetDate()
            {
                var r = gameObject.GetRotate1DLo();
                System.Object[] re = new System.Object[] { r };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                gameObject.SetRotateLo((float?)dates[0]);
            }


        }
    }
}
