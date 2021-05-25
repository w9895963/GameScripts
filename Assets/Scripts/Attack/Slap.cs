using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Global.AttackBundle;
using UnityEngine;

public class Slap : MonoBehaviour
{
    public enum AnimationName { Stop, Run }
    public Data data = new Data();
    [System.Serializable]
    public class Data
    {
        public AttackType attackType = AttackType.HeroDefaultSlap;
        public float lifeTime = 0.6f;
    }



    private void Start()
    {
        RunSlayAnimation();
    }




    private void RunSlayAnimation()
    {
        string runState = AnimationName.Run.ToString();
        Animator animator = GetComponent<Animator>();
        animator.Play(runState, 0, 0);
        float length = animator.GetCurrentAnimatorStateInfo(0).length;

        gameObject.Destroy(length);



    }




    public static void CreateAttack(AttackType type,
                Vector2? position = null, float? angle = null)
    {
        AttackProfile attackProfile = AttackProfile.GetGlobalProfile(type);
        string path = attackProfile.prefabPath;
        Action<GameObject> LoadAction = (attack) =>
        {
            GameObject.Instantiate(attack);
        };
        ResourceLoader.LazyLoad<GameObject>(path, LoadAction);
    }


}
