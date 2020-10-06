using UnityEngine;
using static LayerManager;

public class LayerManager : MonoBehaviour {
    public LayerSettingCs layerSetting = new LayerSettingCs ();
    [System.Serializable] public class LayerSettingCs {
        public int staticSolid = 8;
        public int tempLayer = 31;
    }

    private static LayerSettingCs layerIndex;
    public static LayerSettingCs LayerIndex {
        get {
            if (layerIndex == null) {
                layerIndex = GameObject.FindObjectOfType<LayerManager> ().layerSetting;
            }
            return layerIndex;
        }
        set { layerIndex = value; }

    }

}


namespace Global {

    public static class Layer {
        public static LayerCs staticSolid = new LayerCs (LayerIndex.staticSolid);
        public static LayerCs tempLayer = new LayerCs (LayerIndex.tempLayer);



        public class LayerCs {
            private string name;
            public LayerCs (int index) {
                this.name = LayerMask.LayerToName (index);
            }

            public string Name {
                get => name;
            }
            public int Index {
                get => LayerMask.NameToLayer (name);
            }
            public int Mask {
                get => LayerMask.GetMask (name);
            }
            public ContactFilter2D ContactFilter {
                get {
                    ContactFilter2D filter = new ContactFilter2D ();
                    filter.useLayerMask = true;
                    filter.layerMask = LayerMask.GetMask (name);
                    return filter;
                }
            }
        }
    }
}