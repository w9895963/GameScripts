using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_RenderSortingLayer : CommandLineActionHolder
        {
            static Dictionary<string, string> nameToLayerDic = new Dictionary<string, string>()
            {
                    {"大背景","Background"},
                    {"背景","Back"},
                    {"中景","Default"},
                    {"前景","Fore"},
                    {"大前景","Foreground"},

            };
            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                if (obj == null) { return; }
                var render = obj.GetComponent<Renderer>();
                string layerName = cl.ReadParam<string>(0);
                string sortName = null;
                nameToLayerDic.TryGetValue("layerName", out sortName);
                render.sortingLayerName = sortName;
                if (cl.ParamsLength > 1)
                {
                    int v = cl.ReadParam<int>(1);
                    render.sortingOrder = v;
                }
            }




        }
    }

}
