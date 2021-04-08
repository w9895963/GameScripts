using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInstance : MonoBehaviour
{
    public Data data = new Data();
    [System.Serializable]
    public class Data
    {
        public GameObject attacker;
        public AttackerClass attackerClass = default;
    }

    public enum AttackerClass
    {
        Hero,
        Enemy
    }

}
