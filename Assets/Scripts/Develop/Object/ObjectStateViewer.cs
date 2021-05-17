using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateViewer : MonoBehaviour
{
    public new GameObject gameObject;
    public List<string> dateView;
    private void Update()
    {
        var dates = gameObject.GetComponent<ObjectStateComponent>()?.statesE;
        if (dates == null) { return; }

        dateView = new List<string>();
        dates.ForEach((date) =>
        {
            dateView.Add(date.ToString());
        });



    }
}
