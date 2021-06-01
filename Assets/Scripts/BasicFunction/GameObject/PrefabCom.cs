using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Comp;
using UnityEngine;

namespace PrefabBundle
{


    namespace Comp
    {
        public class PrefabCom : MonoBehaviour
        {
            public string folderPath = "Prefab";
            public string filePath = null;
            public Prefab prefabInst;

            private void Reset()
            {
                GeneratePath();
            }
            [ContextMenu("GeneratePath")]
            public void GeneratePath()
            {
                filePath = $"{folderPath}/{name}";
            }
        }
    }



    public static class ShareDate
    {
        private static List<Comp.PrefabCom> allPrefabCom;

        public static List<PrefabCom> AllPrefabCom
        {
            get
            {
                if (allPrefabCom == null)
                {
                    allPrefabCom = GameObject.FindObjectsOfType<Comp.PrefabCom>(true).ToList();
                }
                return allPrefabCom;
            }
        }

        public static Dictionary<Prefab, string> PrintNameDic = new Dictionary<Prefab, string>()
        {
            {PrefabI.SceneLayer,"场景图层"}
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
                string vs = ShareDate.PrintNameDic.TryGetValue(this);
                return vs.IsEmpty() ? fileName : vs;
            }
        }

        public GameObject originPrefabObject;

        public Prefab()
        {
        }
        public Prefab(string fileName)
        {
            this.fileName = fileName;
        }
        public bool IsPrefabOf(GameObject gameObject)
        {
            PrefabCom prefabCom = gameObject.GetComponent<PrefabCom>();
            if (prefabCom != null)
            {
                if (prefabCom.filePath == filePath)
                    return true;
            }
            return false;
        }


        public GameObject Find()
        {
            List<PrefabCom> allPrefabCom = ShareDate.AllPrefabCom;
            PrefabCom prefabCom = allPrefabCom.Find((x) => x.filePath == filePath);
            if (prefabCom == null) { return null; }
            return prefabCom.gameObject;
        }
        public GameObject[] FindAll()
        {
            List<GameObject> re;
            List<PrefabCom> allPrefabCom = ShareDate.AllPrefabCom;
            List<PrefabCom> prefabCom = allPrefabCom.FindAll((x) => x.filePath == filePath);
            re = prefabCom.Select((x) => x.gameObject).ToList();
            return re.ToArray();
        }


        public GameObject CreateInstance(Action<GameObject> onCreate = null)
        {
            GameObject re = null;
            if (originPrefabObject == null)
            {
                originPrefabObject = ResourceLoader.Load<GameObject>(filePath);
            }
            if (originPrefabObject == null)
            {
                Debug.LogError($"load prefab file error,path:{filePath}");
                return re;
            }
            re = GameObject.Instantiate(originPrefabObject);

            Comp.PrefabCom cm = re.GetComponent<Comp.PrefabCom>();
            if (cm == null)
            {
                Debug.LogError($"Prefab object don't has component: {typeof(Comp.PrefabCom).Name}");
            }
            ShareDate.AllPrefabCom.Add(cm);
            onCreate?.Invoke(re);
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










