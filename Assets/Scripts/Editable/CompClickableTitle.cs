using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;




namespace EditableBundle
{
    namespace Comp
    {
        public class CompClickableTitle : MonoBehaviour
        {
            public Text titleTextComponent;
            public Button buttonComponent;
            public string titleText { set => titleTextComponent.text = value; get => titleTextComponent.text; }
            public void AddOnClickAction(UnityAction onClick)
            {
                buttonComponent.onClick.AddListener(onClick);
            }

        }
    }
}
