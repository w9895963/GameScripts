using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class Data
    {
        private static GameObject gameObject;
        public static GameObject GameObject
        {
            get
            {
                if (gameObject == null)
                {
                    gameObject = GameObject.FindObjectOfType<Player.PlayerObjectMark>().gameObject;
                }
                return gameObject;
            }
        }
        private static Animator animator;
        public static Animator Animator
        {
            get
            {
                if (animator == null)
                {
                    animator = GameObject.GetComponentInChildren<Animator>();
                }
                return animator;
            }
        }
    }
}
