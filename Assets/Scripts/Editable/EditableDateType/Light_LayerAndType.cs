using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System.Linq;
using EditableBundle.Comp;
using System;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;
using static EditableBundle.ShareDate;

namespace EditableBundle
{


    namespace DateType
    {
        public class Light_LayerAndType : EditDate
        {

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "基础灯光属性",
                paramNames = new[] { "灯光种类", "灯光图层" },
                ParamConfigs = new BuildUiConfig.ParamConfig[2]{
                    new BuildUiConfig.ParamConfig(){
                    UiType=BuildUiConfig.ParamUiItem.DropList,
                    dropListContents=lightTypeOptions,
                    dropListValue=() =>(int)GetDate()[0],
                    },
                    new BuildUiConfig.ParamConfig(){
                    UiType=BuildUiConfig.ParamUiItem.DropList,
                    dropListContents=lightLayerOptions,
                    dropListValue=() =>(int)GetDate()[1],
                    },
                },
            };



            private string[] lightTypeOptions = new[]{
                "全局光",
                "点光",
                "法线光",
            };
            private string[] lightLayerOptions = new[]{
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
            };





            public override Type[] DateTypes => new[] { typeof(int), typeof(int) };
            public override System.Object[] GetDate()
            {
                var com = gameObject.GetComponent<Comp.LightManager>();
                if (com == null) return null;
                System.Object[] re = new System.Object[] { (int)com.LightType, (int)com.LightLayer };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                var com = gameObject.GetComponent<Comp.LightManager>();

                int? typeInt = dates[0] as int?;
                if (typeInt != null)
                {
                    com.LightType = (ShareDate.EditableLightType)typeInt;
                }

                int? layerInt = dates[1] as int?;
                if (layerInt != null)
                {
                    com.LightLayer = layerInt.Value;
                }
            }


        }
    }
}
