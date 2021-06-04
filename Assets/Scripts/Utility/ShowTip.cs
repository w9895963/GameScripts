using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowTip : MonoBehaviour
{
    private GameObject tip;
    public string showText;

    void Start()
    {

        BasicEvent.OnPointerHover.Add((GameObject)gameObject,
            (d) =>
            {
                if (tip == null)
                {
                    tip = PrefabI.UI_ToolTip.CreateInstance();
                }
                tip.GetComponentInChildren<Text>().text = showText;

            },
             (d) =>
            {
                if (tip != null)
                {
                    tip.Destroy();
                    tip = null;
                }

            });

    }

}
