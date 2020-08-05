using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TimeScale : MonoBehaviour {
    public bool autoUpdate = true;
    [Range (0, 1)]
    public float timeScale = 1;
    private float lastTimeScale = 1;



    void Update () {
        if (timeScale != lastTimeScale) {
            Time.timeScale = timeScale;
            lastTimeScale = timeScale;
        }

        if (Time.timeScale != timeScale) {
            timeScale = Time.timeScale;
            lastTimeScale = Time.timeScale;
        }
    }
}