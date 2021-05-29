using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIBundle
{
    namespace Item
    {
        public class TableItemEditString : TableItem
        {
            public string title;
            public string content;
            public UnityAction<string> onEndEdit;
            public UnityAction<string> onChanged;

            public override string prefabPath => "Prefab/UI_Editor_EditLine";

            public override void Setup()
            {
                var com = gameObject.GetComponent<Component.EditString>();
                com.content.text = content;
                com.title.text = title;
                if (onEndEdit != null)
                {
                    com.content.onEndEdit.AddListener(onEndEdit);
                }
                if (onChanged != null)
                {
                    com.content.onValueChanged.AddListener(onChanged);

                }


            }

        }
    }

}
