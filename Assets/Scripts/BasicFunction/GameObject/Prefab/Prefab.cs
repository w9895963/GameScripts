using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Compt;
using UnityEngine;

namespace PrefabBundle
{




    public static class ShareDate
    {
        private static List<Compt.PrefabInstance> allPrefabCom;

        public static List<PrefabInstance> AllPrefabComponents
        {
            set => allPrefabCom = value;
            get
            {
                if (allPrefabCom == null)
                {
                    allPrefabCom = GameObject.FindObjectsOfType<Compt.PrefabInstance>(true).ToList();
                }
                return allPrefabCom;
            }
        }


        public static Dictionary<string, string> PrintNameDic = new Dictionary<string, string>()
        {
            { "SceneLayer", "场景图层" },
            { "SceneLayer_Forward", "场景图层-前景" },
            { "SceneLayer_Back", "场景图层-背景" },
            { "LightManager", "光源" },
        };




    }




    public class Prefab
    {
        public string folderPath = "Prefab";
        public string fileName;
        public string filePath => $"{folderPath}/{fileName}";
        public string PrintName
        {
            get
            {
                string re = ShareDate.PrintNameDic.TryGetValue(fileName);
                if (re.IsEmpty())
                {
                    re = this.GetType().Name;
                }
                return re;
            }
        }

        public GameObject originPrefabObject => ResourceLoader.Load<GameObject>(filePath);



        public Prefab()
        {
        }
        public Prefab(string fileName)
        {
            string dirName = Path.GetDirectoryName(fileName);
            if (!dirName.IsEmpty())
            {
                folderPath = dirName;
            }
            this.fileName = Path.GetFileNameWithoutExtension(fileName);
        }



        public bool IsPrefabOf(GameObject gameObject)
        {
            PrefabInstance prefabCom = gameObject.GetComponent<PrefabInstance>();
            if (prefabCom != null)
            {
                if (prefabCom.filePath == filePath)
                    return true;
            }
            return false;
        }


        public GameObject Find()
        {
            List<PrefabInstance> allPrefabCom = ShareDate.AllPrefabComponents;
            PrefabInstance prefabCom = allPrefabCom.Find((x) => x.filePath == filePath);
            if (prefabCom == null) { return null; }
            return prefabCom.gameObject;
        }
        public GameObject[] FindAll()
        {
            List<GameObject> re;

            List<PrefabInstance> allPrefabCom = ShareDate.AllPrefabComponents;
            List<PrefabInstance> prefabCom = allPrefabCom.FindAll((x) => x.filePath == filePath);
            re = prefabCom.Select((x) => x.gameObject).ToList();
            
            return re.ToArray();
        }


        public GameObject CreateInstance(Action<GameObject> onCreate = null)
        {
            GameObject re = null;

            if (originPrefabObject == null)
            {
                Debug.LogError($"load prefab file error,path:{filePath}");
                return re;
            }
            re = GameObject.Instantiate(originPrefabObject);

            Compt.PrefabInstance cm = re.GetComponent<Compt.PrefabInstance>(true);
            if (cm == null)
            {
                Debug.LogError($"Prefab object don't has component: {typeof(Compt.PrefabInstance).Name}");
            }
            ShareDate.AllPrefabComponents.Add(cm);
            onCreate?.Invoke(re);
            return re;
        }
        public GameObject CreateInstance(params System.Object[] paramArray)
        {
            GameObject re = CreateInstance();

            IParams iCom = re.GetComponent<IParams>();
            iCom.Parameters = paramArray;
            return re;
        }
        public GameObject CreateInstance(GameObject parent, Action<GameObject> onCreate = null, bool stay = false)
        {
            GameObject re = CreateInstance(onCreate);
            re.SetParent(parent, stay);
            return re;
        }





    }





}










