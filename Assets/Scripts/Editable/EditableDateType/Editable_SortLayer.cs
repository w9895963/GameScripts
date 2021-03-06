using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System;

namespace EditableBundle
{


    namespace DateType
    {
        public class Editable_SortLayer : EditDate
        {
            static List<(int i, string name)> nameInd = new List<(int i, string name)>()
            {
                (1, "Background"),
                (2, "Back"),
                (3, "Default"),
                (4, "Fore"),
                (5, "Foreground"),
            };

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "图层属性",
                paramNames = new[] { "图层", "层内顺序" },
            };

            public override Type[] DateTypes => new[] { typeof(int), typeof(int) };
            public override System.Object[] GetDate()
            {
                var com = gameObject.GetComponent<Renderer>();
                if (com == null) return null;
                string layerName = com.sortingLayerName;
                int layerIndex = nameInd.Find((x) => x.name == layerName).i;
                int order = com.sortingOrder;
                System.Object[] re = new System.Object[] { layerIndex, order };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                var com = gameObject.GetComponent<Renderer>();
                int? intV = (int?)dates[0];

                if (intV != null)
                {
                    com.sortingLayerName = nameInd.Find((x) => x.i == intV.Value).name;
                }
                int? intV2 = (int?)dates[1];
                if (intV2 != null)
                {
                    com.sortingOrder = intV2.Value;
                }

            }


        }
    }
}
