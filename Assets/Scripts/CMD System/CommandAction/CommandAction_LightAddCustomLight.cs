using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightAddCustomLight : CommandActionHolder
        {
            const string pathHead = "CommandPrefab/Light/CustomLight/Light";

            static List<LightPrefabDate> lightUseOrder = new List<LightPrefabDate>();
            static List<int> noNewPref = new List<int>();


            public override void Action(CommandLine cl)
            {
                int lightLayer = 0;
                string texPath = null;
                if (cl.ParamsLength == 2)
                {
                    lightLayer = cl.ReadParam<int>(0);
                    texPath = FileF.GetFilePathInSameFolder(cl.Path, cl.ReadParam<string>(1));
                }
                else
                {
                    return;
                }


                LightPrefabDate LPR = GetExistLightPref(lightLayer, texPath);
                if (LPR == null)
                {
                    LPR = TryLoadNewLightPref(lightLayer, texPath);
                    if (LPR == null)
                    {
                        LPR = GetOldestPref(lightLayer);
                        if (LPR == null)
                        {
                            return;
                        }
                    }

                }
                GameObject light = GameObject.Instantiate(LPR.prefab);
                UpdateUseOrder(LPR);

                cl.GameObject = light;
                SceneF.AddToScene(light, cl.SceneName);
            }

            class LightPrefabDate
            {
                public GameObject prefab;
                public int layer;
                public int index;
                public string texPath;
            }

            void CreateLight(LightPrefabDate pref) { }

            LightPrefabDate GetExistLightPref(int layerIndex, string textPath)
            {
                return lightUseOrder.Find((x) =>
                {
                    if (x.layer == layerIndex)
                    {
                        if (x.texPath == textPath)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                });

            }


            LightPrefabDate TryLoadNewLightPref(int layer, string texPath)
            {
                if (noNewPref.Contains(layer))
                {
                    return null;
                }
                else
                {
                    int count = lightUseOrder.Count((x) => x.layer == layer);
                    GameObject pr = ResourceLoader.Load<GameObject>(GetPath(layer, count));
                    byte[] vs = File.ReadAllBytes(texPath);
                    pr.GetComponent<Light2D>().lightCookieSprite.texture.LoadImage(vs);
                    if (pr != null)
                    {
                        return new LightPrefabDate()
                        {
                            prefab = pr,
                            layer = layer,
                            index = count,
                            texPath = texPath,
                        };
                    }
                    else
                    {
                        noNewPref.Add(layer);
                        return null;
                    }

                }
            }
            private LightPrefabDate GetOldestPref(int layer)
            {
                return lightUseOrder.FindLast((x) => x.layer == layer);
            }
            void UpdateUseOrder(LightPrefabDate prDate)
            {
                lightUseOrder.Remove(prDate);
                lightUseOrder.Insert(0, prDate);
            }

            string GetPath(int layer, int index)
            {
                return $"{pathHead} {layer} {index}";
            }


        }
    }

}
