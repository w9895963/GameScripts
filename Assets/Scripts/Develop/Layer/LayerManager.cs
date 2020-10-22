using UnityEngine;
using static LayerManager;
using System.Collections.Generic;
using Global;
using static Global.LayerUtility;

public class LayerManager : MonoBehaviour {


    private void Awake () {
        Function.FindAllInterfaces<ILayer> ().ForEach ((comp) => {
            (comp as MonoBehaviour).gameObject.layer = comp.LayerIndex;
        });
    }

}