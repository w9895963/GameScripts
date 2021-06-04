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
        public class MoveWithCamera : EditDate
        {

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "跟随相机",
                paramNames = new[] { "X", "Y" },
            };









            public override Type[] DateTypes => new[] { typeof(float), typeof(float) };
            public override System.Object[] GetDate()
            {
                var com = gameObject.GetComponent<global::Utility.MoveWithCamera>();
                if (com == null) return null;
                System.Object[] re = new System.Object[] { com.moveRate.x, com.moveRate.y };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                var com = gameObject.GetComponent<global::Utility.MoveWithCamera>(true);


                var x = dates[0] as float?;

                var y = dates[1] as float?;


                Vector2 m = com.moveRate;
                com.moveRate = new Vector2(x ?? m.x, y ?? m.y);
            }


        }
    }
}
