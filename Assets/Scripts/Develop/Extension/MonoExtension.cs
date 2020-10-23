using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviour_Extension {
    public static void DestroyChildren (this MonoBehaviour mono) {
        int childCount = mono.
        transform.
        childCount;
        for (int i = 0; i < childCount; i++) {
            GameObject.Destroy (mono.transform.GetChild (i).gameObject);
        }
    }
    public static GameObject CreateChildrenFrom (this MonoBehaviour mono, GameObject source) {
        GameObject gameObject = GameObject.Instantiate (source, mono.transform);
        return gameObject;
    }
}