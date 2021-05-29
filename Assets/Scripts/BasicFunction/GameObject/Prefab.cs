using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrefabBundle
{


    namespace Component
    {
        public class Prefab : MonoBehaviour
        {
            public string prefabPath;
        }
    }






    public static class PrefabCreator
    {
        public static List<GameObject> AllPrefabInstants;
        public static GameObject FindOrCreate(string prefabPath, GameObject parent = null)
        {
            if (AllPrefabInstants == null)
            {
                Component.Prefab[] prefabs = GameObject.FindObjectsOfType<Component.Prefab>();
                AllPrefabInstants = new List<GameObject>(prefabs.Select((x) => x.gameObject));
            }

            AllPrefabInstants.RemoveNull();
            GameObject marchObject = AllPrefabInstants.Find((x) =>
               {
                   Component.Prefab prefab = x.GetComponent<Component.Prefab>();
                   GameObject gameObject = prefab.gameObject;
                   bool prefabSame = prefab.prefabPath == prefabPath;
                   bool childTest = gameObject.IsChildOf(parent);
                   return prefabSame & childTest;
               });

            if (marchObject == null)
            {
                GameObject gameObject = GameObjectF.CreateFromPrefab(prefabPath);
                gameObject.GetComponent<Component.Prefab>(true).prefabPath = prefabPath;
                gameObject.SetParent(parent, false);
                return gameObject;
            }
            else
            {
                return marchObject.gameObject;
            }

        }

    }

}


