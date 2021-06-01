using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;




namespace EditableBundle
{
    namespace Comp
    {
        public class CompItemClickableTitle : MonoBehaviour
        {
            public Text titleTextComponent;
            public Button buttonComponent;
            public string titleText { set => titleTextComponent.text = value; get => titleTextComponent.text; }
            public void AddClickAction(UnityAction onClick)
            {
                buttonComponent.onClick.AddListener(onClick);
            }

        }
    }
}
