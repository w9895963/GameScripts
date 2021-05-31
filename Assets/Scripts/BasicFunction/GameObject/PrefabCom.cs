using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using PrefabBundle.Component;
using UnityEngine;

namespace PrefabBundle
{


    namespace Component
    {
        public class PrefabCom : MonoBehaviour
        {
            public string folderPath = "Prefab";
            public string filePath = null;
            public Prefab prefabInst;

            private void Reset()
            {
                filePath = filePath.IsEmpty() ? $"{folderPath}/{name}" : filePath;
            }
        }
    }



    public static class StaticDate
    {
        private static List<Component.PrefabCom> allPrefabCom;

        public static List<PrefabCom> AllPrefabCom
        {
            get
            {
                if (allPrefabCom == null)
                {
                    allPrefabCom = GameObject.FindObjectsOfType<Component.PrefabCom>(true).ToList();
                }
                return allPrefabCom;
            }
        }
    }




    public class Prefab
    {
        public string folderPath = "Prefab";
        public string fileName;
        public string filePath => $"{folderPath}/{fileName}";
        public GameObject originPrefabObject;

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
            List<PrefabCom> allPrefabCom = StaticDate.AllPrefabCom;
            PrefabCom prefabCom = allPrefabCom.Find((x) => x.filePath == filePath);
            if (prefabCom == null) { return null; }
            return prefabCom.gameObject;
        }
        public GameObject[] FindAll()
        {
            List<GameObject> re;
            List<PrefabCom> allPrefabCom = StaticDate.AllPrefabCom;
            List<PrefabCom> prefabCom = allPrefabCom.FindAll((x) => x.filePath == filePath);
            re = prefabCom.Select((x) => x.gameObject).ToList();
            return re.ToArray();
        }

        public GameObject CreateInstance()
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

            Component.PrefabCom cm = re.GetComponent<Component.PrefabCom>();
            if (cm == null)
            {
                Debug.LogError($"Prefab object don't has component: {typeof(Component.PrefabCom).Name}");
            }
            StaticDate.AllPrefabCom.Add(cm);
            return re;
        }
        public GameObject CreateInstance(GameObject parent, bool stay = false)
        {
            GameObject re = CreateInstance();
            re.SetParent(parent, stay);
            return re;
        }


    }



}










