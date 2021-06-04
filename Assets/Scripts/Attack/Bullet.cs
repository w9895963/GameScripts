using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Global;

public class Bullet : MonoBehaviour
{
    public static List<Bullet> AllBullets { get => allBullets; }
    public ParticleSystem ParticleSystem { get => particleSystem; }
    // * ---------------------------------- 
    private static List<Bullet> allBullets = new List<Bullet>();
    // * ---------------------------------- 
    private Action<HitData> onParticleTriggerEnter;
    private new ParticleSystem particleSystem;



    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        allBullets.Add(this);

    }
    private void OnDestroy()
    {
        allBullets.Remove(this);
    }

    private void OnParticleTrigger()
    {
        ParticleSystem ptc = GetComponent<ParticleSystem>();
        List<ParticleSystem.Particle> hitLIst = new List<ParticleSystem.Particle>();
        ParticleSystem.ColliderData cldD;
        int hitCount = ptc.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, hitLIst, out cldD);
        if (hitCount > 0)
        {

            HitData hitData = new HitData(hitLIst, cldD, ptc);


            if (onParticleTriggerEnter != null)
            {
                onParticleTriggerEnter.Invoke(hitData);
            }


            if (hitData.IsParticleModified)
            {

                ptc.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, hitData.Particles);
            }
        }

    }





    public class HitData
    {
        private List<ParticleSystem.Particle> particleList;
        private ParticleSystem.ColliderData colliderData;
        private ParticleSystem particleSystem;
        private Dictionary<Component, List<ParticleSystem.Particle>> colliderHitDic =
                new  Dictionary<Component, List<ParticleSystem.Particle>>();
        private bool particleModified = false;


        public HitData(List<ParticleSystem.Particle> particleList,
                       ParticleSystem.ColliderData colliderData, ParticleSystem particleSystem)
        {
            this.particleList = particleList;
            this.colliderData = colliderData;
            this.particleSystem = particleSystem;
            var cldD = colliderData;
            var dic = colliderHitDic;

            // *Set colliderData
            for (int particleIndex = 0; particleIndex < particleList.Count; particleIndex++)
            {
                int colliderCount = cldD.GetColliderCount(particleIndex);
                ParticleSystem.Particle particle = particleList[particleIndex];

                for (int colliderIndex = 0; colliderIndex < colliderCount; colliderIndex++)
                {
                    Component coliderCom = cldD.GetCollider(particleIndex, colliderIndex);
                    if (coliderCom != null)
                    {
                        bool keyExist = dic.ContainsKey(coliderCom);
                        if (keyExist)
                        {
                            dic[coliderCom].Add(particle);
                        }
                        else
                        {
                            dic.Add(coliderCom, new List<ParticleSystem.Particle>());
                        }
                    }

                }
            }
        }

        public void SetParticle(Func<ParticleSystem.Particle, ParticleSystem.Particle> setAction,
                                List<Component> hitColliders = null)
        {

            var cldData = colliderData;
            for (int ptcI = 0; ptcI < particleList.Count; ptcI++)
            {
                if (hitColliders == null)
                {
                    particleList[ptcI] = setAction(particleList[ptcI]);
                    particleModified = true;
                }
                else
                {
                    int cldC = cldData.GetColliderCount(ptcI);
                    for (int CldI = 0; CldI < cldC; CldI++)
                    {
                        Component cld = cldData.GetCollider(ptcI, CldI);
                        if (hitColliders.Contains(cld))
                        {
                            particleList[ptcI] = setAction(particleList[ptcI]);
                            particleModified = true;
                        }
                    }
                }


            }
        }

        public int GetHitCount(Component collider)
        {
            int result = 0;
            bool v = colliderHitDic.ContainsKey(collider);
            if (v)
            {
                result = colliderHitDic[collider].Count;
            }
            return result;
        }


        public bool IsParticleModified => particleModified;
        public ParticleSystem ParticleSystem => particleSystem;
        public List<ParticleSystem.Particle> Particles => particleList;
        public List<Component> GetColliders()
        {
            return colliderHitDic.Keys.ToList();
        }
        public List<ParticleSystem.Particle> GetParticles(Component collider)
        {
            return colliderHitDic[collider];
        }

    }

    public void AddParticleTriggerEnterAction(Action<HitData> action)
    {
        onParticleTriggerEnter += action;
    }
    public void RemoveParticleTriggerEnterAction(Action<HitData> action)
    {
        onParticleTriggerEnter -= action;
    }

    public void Shot()
    {
        particleSystem.Play();
    }
}


