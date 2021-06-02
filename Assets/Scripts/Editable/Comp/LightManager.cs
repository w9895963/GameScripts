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
        public class LightManager : MonoBehaviour
        {
            [SerializeField]
            private int lightLayer = 0;
            [SerializeField]
            private EditableLightType lightType = EditableLightType.GlobalLight;
            private string globalLightFolder = "Prefab/Light/GlobalLight";
            private string pointLightFolder = "Prefab/Light/PointLight";
            private string NormalLightFolder = "Prefab/Light/NormalLight";

            public int LightLayer
            {
                get => lightLayer;
                set
                {
                    int lightLayerOld = lightLayer;
                    lightLayer = value;
                    if (lightLayer != lightLayerOld)
                    {
                        ChangeLight();
                    }

                }
            }
            public EditableLightType LightType
            {
                get => lightType;
                set
                {
                    EditableLightType lightTypeOld = lightType;
                    lightType = value;
                    if (lightTypeOld != lightType)
                    {
                        ChangeLight();
                    }
                }
            }

            public GameObject ChangeLight()
            {
                EditDate[] editDates = EditableF.GetALlEditableDate(gameObject);
                object[][] date = editDates.Select((x) => x.GetDate()).ToArray();

                gameObject.GetComponentInChildren<Light2D>().gameObject.DestroyImmediate();


                GameObject re = null;
                if (LightType == EditableLightType.GlobalLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = globalLightFolder;
                    light.fileName = $"Light {LightLayer}";
                    DestroySameLayerGlobalLight();
                    re = light.CreateInstance(gameObject);
                }
                else if (LightType == EditableLightType.PointLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = pointLightFolder;
                    light.fileName = $"Light {LightLayer}";

                    re = light.CreateInstance(gameObject);
                }
                else if (LightType == EditableLightType.NormalLight)
                {
                    Prefab light = new Prefab();
                    light.folderPath = NormalLightFolder;
                    light.fileName = $"Light {LightLayer}";

                    re = light.CreateInstance(gameObject);
                }

                editDates.ForEach((d, i) => d.ApplayDate(date[i]));
                return re;

                void DestroySameLayerGlobalLight()
                {
                    List<LightManager> compLights = GameObject.FindObjectsOfType<LightManager>().ToList();
                    compLights.Remove(gameObject.GetComponent<LightManager>());
                    compLights.Where((x) =>
                    {
                        bool v = x.LightLayer == 0 | x.LightLayer == LightLayer;
                        return v;
                    }).ForEach((com) =>
                    {
                        com.gameObject.Destroy();
                    });
                }
            }


        }


    }
}
