using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System;

namespace EditableBundle
{


    namespace DateType
    {
        public class Editable_Scale : EditDate
        {
            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "缩放",
                paramNames = new[] { "X", "Y" },
            };

            public override Type[] DateTypes => new[] { typeof(float), typeof(float) };
            public override System.Object[] GetDate()
            {
                Vector2 p = gameObject.GetScale2dLo();
                System.Object[] re = new System.Object[] { p.x, p.y };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                gameObject.SetScaleLo((float?)dates[0], (float?)dates[1]);
            }


        }
    }
}
