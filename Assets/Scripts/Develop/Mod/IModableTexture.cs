using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public interface IModableTexture {
        List<Texture2D> ModableTexture { set; get; }
    }
}
