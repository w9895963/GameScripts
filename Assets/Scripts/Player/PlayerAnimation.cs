using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum AnimationState
    {
        Stand,
        Walk,
        Slap,
        Shot,
    }
    public static class Animation
    {

        public static void Play(AnimationState state)
        {
            Player.Data.Animator.Play(state.ToString());
        }
    }
}
