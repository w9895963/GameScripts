using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIF
{

    public static float ChildrenHeight(GameObject parent)
    {
        List<GameObject> children = parent.GetAllChild();
        float height = 0;
        children.ForEach((x) => height += x.GetComponent<RectTransform>().rect.height);
        return height;
    }


    public static GameObject GetCanvas()
    {
        return GameObjectF.FindOrCretePrefab(UIBundle.ShareDate.canvasPrefabPath);
    }

}
