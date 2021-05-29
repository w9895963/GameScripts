using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIBundle
{
    namespace Item
    {
        public class TableItemTitle : TableItem
        {
            public string text;

            public override string prefabPath => "Prefab/UI_Editor_TItletem";

            public override void Setup()
            {
                Component.TableItemText com = gameObject.GetComponent<Component.TableItemText>();
                com.text.text = text;

            }

        }
    }

}
