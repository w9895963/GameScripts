using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Global {
    namespace Animation {
        public static class AnimationUtility {
            public static void SetAnimation (GameObject gameObject, AnimationObject animation) {
                AnimationHolder holder = gameObject.GetComponentInChildren<AnimationHolder> ();
                AnimationObject comp = holder.GetComponentInChildren<AnimationObject> ();
                if (comp != null) {
                    GameObject pref = ObjectData.Get<GameObject> (comp.gameObject, "prefab");
                    if (pref == animation.gameObject) {
                        return;
                    }
                }
                holder.DestroyChildren ();
                GameObject obj = holder.CreateChildrenFrom (animation.gameObject);
                ObjectData.Add (obj, "prefab", animation.gameObject);

            }

        }
    }
}