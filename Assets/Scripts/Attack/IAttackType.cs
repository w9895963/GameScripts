using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global
{
    namespace AttackBundle
    {
        public interface IAttackType
        {
            AttackType AttackType { get; }
            GameObject GameObject { get; }
        }
    }
}

