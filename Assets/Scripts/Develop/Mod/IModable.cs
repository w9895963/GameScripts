using System.Collections;
using System.Collections.Generic;
using Global.Mods;
using UnityEngine;

namespace Global {
    public interface IModable {
        string ModTitle { get; }
        bool EnableWriteModDatas { get; }
        void LoadModData (ModData data);
        System.Object ModableObjectData { get; }


    }
}