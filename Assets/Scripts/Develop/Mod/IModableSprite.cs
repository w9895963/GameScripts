using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
    public interface IModableSprite {
        List<Sprite> ModableSprites { set; get; }
    }
}