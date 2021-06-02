using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolBarToolTip : MonoBehaviour
{
    public string[] toolTips = new[] {
        "选择物体",
        "创建物体",
     };
    private GameObject parent;
    [SerializeField]
    private GameObject curr;
    private GameObject tip;

    void Start()
    {
        List<GameObject> gameObjects = gameObject.GetDirectChildren();
        gameObjects.ForEach((obj, i) =>
        {
            BasicEvent.OnPointerHover.Add(obj,
            (d) =>
            {
                curr = obj;
                if (tip == null)
                {
                    tip = PrefabI.UI_EditorItem_ToolTip.CreateInstance(PrefabI.UI_Editor.Find());
                }
                tip.GetComponentInChildren<Text>().text = toolTips[i];
                var position = Mouse.current.position.ReadValue();
                tip.SetPosition(position);



            },
             (d) =>
            {
                if (curr == obj)
                {
                    curr = null;
                }
                if (tip != null)
                {
                    tip.Destroy();
                    tip = null;
                }

            });
        });
    }

}
