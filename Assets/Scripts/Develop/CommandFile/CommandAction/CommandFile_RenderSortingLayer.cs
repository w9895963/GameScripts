using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class RenderSortingLayer
        {
            static Dictionary<string, string> nameToLayerDic = new Dictionary<string, string>()
            {
                    {"背景3","Background3"},
                    {"背景2","Background2"},
                    {"背景1","Background1"},
                    {"中景","Default"},
                    {"前景1","Foreground1"},
                    {"前景2","Foreground2"},
                    {"前景3","Foreground3"},

            };
            public static void Act(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                var render = obj.GetComponent<Renderer>();
                string layerName = cm.ReadParam<string>(0);
                render.sortingLayerName = nameToLayerDic[layerName];
                if (cm.paramaters.Length > 1)
                {
                    int v = cm.ReadParam<int>(1);
                    render.sortingOrder = v;
                }


            }
        }
    }
}
