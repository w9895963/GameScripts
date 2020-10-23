using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class LayerSetter : MonoBehaviour, ILayer {
    public LayerUtility.PresetLayer layer;
    public int LayerIndex => layer.ToLayer ().Index;


}