using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class ShowIndicator
        {
            const string colliderPath = "CommandPrefab/方形刚体";

            public static void Act(CommandLine cm)
            {
                CamareF.SetIndicatorVisible();

            }
        }
    }
}
