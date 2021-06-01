using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;

namespace EditableBundle
{
    

    namespace DateType
    {
        public class Editable_Name : EditDate
        {
            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "名字属性",
                paramNames = new[] { "名字" },
            };


            public override System.Object[] GetDate()
            {
                string p = gameObject.name;
                System.Object[] re = new System.Object[] { p };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                string v = (string)dates[0];
                gameObject.name = v.IsEmpty() ? gameObject.name : v;
            }


        }
    }
}
