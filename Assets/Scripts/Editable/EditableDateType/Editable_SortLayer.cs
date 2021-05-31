using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;

namespace EditableBundle
{
    namespace EditDateGenerator
    {
        public class SortLayerDateGen : SingleGen
        {
            public override EditDate EditDateGen(GameObject gameObject)
            {
                if (!gameObject.HasComponent<Renderer>()) return null;
                return new Editable_SortLayer();
            }
        }
    }

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

            public override BuildUiConfig BuildUiConfig => new BuildUiConfig()
            {
                title = "图层属性",
                paramNames = new[] { "图层", "层内顺序" },
            };


            public override System.Object[] GetDate()
            {
                var com = gameObject.GetComponent<Renderer>();
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
