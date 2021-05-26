using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CMDBundle
{
    namespace Editor
    {
        public class UtilityClass : MonoBehaviour
        {
            public T FindUp<T>() => gameObject.GetComponentInParent<T>();
            public T Find<T>() => gameObject.GetComponentInChildren<T>();


        }
    }

}
