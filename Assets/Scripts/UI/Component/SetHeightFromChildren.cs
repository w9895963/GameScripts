    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace UIBundle
{
    public class SetHeightFromChildren : MonoBehaviour
    {
        public void UpdateHeight()
        {
            List<GameObject> children = gameObject.GetAllChild();
            float height = 0;
            children.ForEach((x) => height += x.GetComponent<RectTransform>().rect.height);
            RectTransform re = gameObject.GetComponent<RectTransform>();
            re.sizeDelta = new Vector2(re.sizeDelta.x, height);
        }



    }
}
