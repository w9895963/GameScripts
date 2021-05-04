using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;

public class SpriteAnimation : MonoBehaviour {

    public Sprite[] sprites = new Sprite[1];
    public float duration = 0;
    public bool loop = false;
    public AnimationCurve timeCurve = Curve.ZeroOne;

   


}