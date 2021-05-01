using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Global
{
    namespace CharacterBundle
    {
        public static class CharacterAnimation
        {

            public static string paramaterName = "StateIndex";
            public enum State
            {
                Stand,
                Attack,
                Walking,
                Shot,
            }

            public static void Play(GameObject gameObject, State state)
            {
                Animator animator = gameObject.GetComponentInChildren<CharacterAnimatorLo>()?.GetComponent<Animator>();
                if (animator == null)
                { return; }

                string stateName = state.ToString();

                AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
                bool animationExist = animationClips.Any((clip) => clip.name == stateName);
                if (animationExist)
                {
                    animator.Play(stateName);
                }
            }

        }
    }
}
