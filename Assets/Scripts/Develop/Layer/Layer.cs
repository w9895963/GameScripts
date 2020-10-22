using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {

    public static class LayerUtility {
        
        public enum PresetLayer {
            Lead = 9,
            Obstacle = 8,
            TempLayer = 31
        }
        public static Layer Lead = PresetLayer.Lead.ToLayer ();
        public static Layer obstacle = PresetLayer.Obstacle.ToLayer ();
        public static Layer temp = PresetLayer.TempLayer.ToLayer ();

        public static Layer ToLayer (this PresetLayer preset) {
            return new Layer ((int) preset);
        }


        public class Layer {
            private int index;
            public Layer (int index) {
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