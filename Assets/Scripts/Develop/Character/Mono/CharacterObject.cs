using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Character;
using Global.Animate;
using UnityEngine;

public class CharacterObject : CharacterClass {
    public CharacterAnimation Animation;
    void Start () {
        CharacterAnimation cha = gameObject.CreateChild (Animation.gameObject).GetComponentInChildren<CharacterAnimation> ();
        Material material = AnimateUtility.CloneMaterial (cha.gameObject);
        AnimateUtility.SetSpriteMaterialTexture (cha.gameObject, GetComponent<SpriteRenderer> ().sprite.texture);
        GetComponent<SpriteRenderer> ().enabled = false;
    }

}