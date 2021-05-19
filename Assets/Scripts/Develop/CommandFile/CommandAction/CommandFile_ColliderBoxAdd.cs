using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class ColliderBoxAdd
        {
            const string colliderPath = "CommandPrefab/方形刚体";

            public static void Act(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }

                GameObject colliderObj = obj.CreateChildFrom(colliderPath);
                float[] pas = cm.ReadParams<float>();
                if (pas.Length >= 2)
                {
                    colliderObj.SetPosition(pas[0], pas[1]);
                }
                if (pas.Length >= 4)
                {
                    colliderObj.SetScale(pas[2], pas[3]);
                }
                if (pas.Length >= 5)
                {
                    colliderObj.SetRotation(pas[4]);
                }

            }
        }
    }
}
