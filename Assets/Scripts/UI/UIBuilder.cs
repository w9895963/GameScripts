using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UIBundle
{
    public class UITopBuilder
    {
        public string prefabPath;
        public GameObject topObject;
        public List<UIReplacer> replacers = new List<UIReplacer>();
        public void Build()
        {
            topObject = UIF.GetCanvas().CreateChildFrom(prefabPath);

        }
    }
    public class UIReplacer
    {
        public GameObject topBuilder;
        public string prefabPath;
        public void Build()
        {
            GameObject buildObj = UIF.GetCanvas().CreateChildFrom(prefabPath);

        }
    }
    public class UIObject
    {
        public string prefabPath;
        public void Build()
        {
            GameObject buildObj = UIF.GetCanvas().CreateChildFrom(prefabPath);

        }
    }
}
