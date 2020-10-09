using UnityEngine;
using static LayerManager;
using System.Collections.Generic;
using Global;
using static Global.Layer;

public class LayerManager : MonoBehaviour {
    public List<LayerSetCs> layerSetting = new List<LayerSetCs> {
        new LayerSetCs (LayerEnum.staticSolid, 8),
        new LayerSetCs (LayerEnum.tempLayer, 31)
    };

    [System.Serializable] public class LayerSetCs {
        public LayerEnum layer;
        public int index;

        public LayerSetCs (Layer.LayerEnum layer, int index) {
            this.layer = layer;
            this.index = index;
        }
    }
}


namespace Global {

    public static class Layer {
        public static LayerCs staticSolid = LayerEnum.staticSolid.ToLayer ();
        public static LayerCs tempLayer = LayerEnum.tempLayer.ToLayer ();
        public enum LayerEnum { staticSolid, tempLayer }
        public static LayerCs ToLayer (this LayerEnum layerEnum) {
            LayerManager layerManager = GameObject.FindObjectOfType<LayerManager> ();
            int index = layerManager.layerSetting.Find ((x) => x.layer == layerEnum).index;
            return new LayerCs (index);
        }

        public class LayerCs {
            private int index;
            public LayerCs (int index) {
                this.index = index;
            }

            public string Name {
                get => LayerMask.LayerToName (index);
            }
            public int Index {
                get => index;
            }
            public int Mask {
                get => LayerMask.GetMask (Name);
            }
            public ContactFilter2D ContactFilter {
                get {
                    ContactFilter2D filter = new ContactFilter2D ();
                    filter.useLayerMask = true;
                    filter.layerMask = LayerMask.GetMask (Name);
                    return filter;
                }
            }
        }
    }
}