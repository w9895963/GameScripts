using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public interface IModable {
        string ModDataName { get; }
        bool EnableWriteModDatas { get; }
        string ModData { get; }
        void LoadModData (string data);

    }
}