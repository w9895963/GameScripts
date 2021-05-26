using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle.Component;
using UnityEngine;

namespace PrefabBundle
{
    namespace Component
    {
        public class Prefab : MonoBehaviour
        {
            public GameObject prefab;
        }
    }

    namespace Date
    {
        public class PrefabObject
        {

        }
    }



    public static class PrefabCreator
    {
        public static GameObject FindOrCrete(string prefabPath)
        {
            GameObject prefab = ResourceLoader.Load<GameObject>(prefabPath);

            GameObject inst = DateF.FindObjectOfDate<GameObject, Date.PrefabObject, GameObject>(prefab);
            if (inst == null)
            {
                inst = GameObject.Instantiate(prefab);
                DateF.AddDate<Date.PrefabObject, GameObject>(inst, prefab);
            }



            return inst;
        }
    }

}


