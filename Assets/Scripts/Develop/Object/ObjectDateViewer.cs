using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectDateViewer : MonoBehaviour
{
    public new GameObject gameObject;
    public List<string> date;
    private void Update()
    {
        var dateDic = gameObject.GetComponent<ObjectDateComponent>()?.dateDict;
        if (dateDic == null) { return; }
        date = new List<string>();
        foreach (var key in dateDic.Keys)
        {
            string keyname = key.ToString();
            string dateSt = dateDic[key].ToString();
            string str = $"{keyname}:{dateSt}";
            date.Add(str);
        }

    }

}
