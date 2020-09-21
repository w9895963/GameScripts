using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public static class Layer {
        public static NormalLayer tempLayer = new NormalLayer ("Temp");

        public class NormalLayer {
            private string name;
            public NormalLayer (string name) {
                this.name = name;
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
        }
    }

}