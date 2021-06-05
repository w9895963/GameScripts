using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Utility
{

    public class OnSceneLoad_StaticDateReset : MonoBehaviour
    {
        private void Awake()
        {
            PrefabBundle.ShareDate.AllPrefabComponents = null;
        }
    }


}
