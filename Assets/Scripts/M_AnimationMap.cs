using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class M_AnimationMap : MonoBehaviour {
    [SerializeField]
    private PlayableDirector timeline = null;
    [SerializeField]
    private GameObject referenceObject = null;
    [SerializeField]
    private Vector2 positionStart = default;
    [SerializeField]
    private Vector2 positionEnd = default;


    void Update () {
        Vector2 position = referenceObject.transform.position;

        float length = (float) timeline.duration;

        float rate = (position - positionStart).magnitude / (positionEnd - positionStart).magnitude;


        timeline.time = rate * length;
    }
}