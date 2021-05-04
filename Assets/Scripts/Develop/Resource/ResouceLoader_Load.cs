using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResouceLoaderBundle
{
    public static class ResouceLoader_Load
    {
        public static Dictionary<string, Object> loadObjDic = new Dictionary<string, Object>();
        public static T Load<T>(string path, bool reload = false) where T : Object
        {
            Object obj = null;
            bool isExist = loadObjDic.TryGetValue(path, out obj);
            if (!isExist)
            {
                obj = Resources.Load(path, typeof(T));
                if (obj != null)
                {
                    loadObjDic.Add(path, obj);
                }
            }
            else
            {
                if (reload)
                {
                    obj = Resources.Load(path, typeof(T));
                    if (obj == null)
                    {
                        loadObjDic.Remove(path);
                    }
                    else
                    {
                        loadObjDic[path] = obj;
                    }
                }
            }

            return (T)obj;

        }
    }
}
