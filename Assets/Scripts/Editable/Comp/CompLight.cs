using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using static EditableBundle.ShareDate;

namespace EditableBundle
{
    namespace Comp
    {
        public class CompLight : MonoBehaviour
        {
            public int lightLayer = 0;
            public EditableLightType lightType = EditableLightType.GlobalLight;
            private string globalLightFolder = "Prefab/Light/GlobalLight";
            private string pointLightFolder = "Prefab/Light/PointLight";
            private string NormalLightFolder = "Prefab/Light/NormalLight";

            public GameObject CreateLightAndDestroySelf()
            {
                gameObject.Destroy();
                GameObject re = null;
                if (lightType == EditableLightType.GlobalLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = globalLightFolder;
                    light.fileName = $"Light {lightLayer}";
                    DestroySameLayerGlobalLight();
                    re = light.CreateInstance();
                    AfterCreate(re);
                }
                else if (lightType == EditableLightType.PointLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = pointLightFolder;
                    light.fileName = $"Light {lightLayer}";

                    re = light.CreateInstance();
                    AfterCreate(re);
                }
                else if (lightType == EditableLightType.NormalLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = NormalLightFolder;
                    light.fileName = $"Light {lightLayer}";

                    re = light.CreateInstance();
                    AfterCreate(re);
                }

                return re;

                void DestroySameLayerGlobalLight()
                {
                    CompLight[] compLights = GameObject.FindObjectsOfType<CompLight>();
                    compLights.Where((x) =>
                    {
                        bool v = x.lightLayer == 0 | x.lightLayer == lightLayer;
                        return v;
                    }).ForEach((com) =>
                    {
                        com.gameObject.Destroy();
                    });
                }
            }

            private void AfterCreate(GameObject re)
            {
                re.GetComponent<CompEditableObject>(true);
                CompLight com = re.GetComponent<CompLight>(true);
                com.lightLayer = lightLayer;
                com.lightType = lightType;
            }
        }


    }
}
