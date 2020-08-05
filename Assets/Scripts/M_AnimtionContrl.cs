using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_AnimtionContrl : MonoBehaviour {

    public Animation animationComponent;

    public bool PhysicsMode = false;
    public bool Loop = false;
    public bool realTimePlay = false;

    [Header ("Main Variable:PlaySpeed")]
    [SerializeField]
    [Range (-1, 1)]
    private float playSpeed = 0;
    private float startTime = 0;
    private float clipStartTime = 0;
    private float lastPlaySpeed;
    private float currClipTime;
    private float lastTime;
    private float time;

    void Start () {
        animationComponent.Stop ();
    }
    void Update () {
        if (!PhysicsMode) Main ();
    }


    private void FixedUpdate () {
        if (PhysicsMode) Main ();
    }

    private void Main () {

        time = realTimePlay?Time.realtimeSinceStartup : Time.time;

        if (playSpeed != 0) {


            float length = animationComponent.clip.length;


            bool playStateChanged = lastPlaySpeed != playSpeed;
            if (playStateChanged) {
                clipStartTime = currClipTime;
                startTime = lastTime;
            }



            if (Loop) currClipTime = ((clipStartTime + (time - startTime) * playSpeed)
                    % length + length)
                % length;
            else currClipTime = Mathf.Clamp (clipStartTime + (time - startTime) * playSpeed, 0, length);
            animationComponent.clip.SampleAnimation (animationComponent.gameObject, currClipTime);



        }


        lastPlaySpeed = playSpeed;
        lastTime = time;
    }

    public float GetPlayDegree () {
        return currClipTime / animationComponent.clip.length;
    }

    public void PlayFromStart () {
        currClipTime = 0;
        lastPlaySpeed = 0;
        playSpeed = 1;
    }
    public void PlayForward (float speed) {
        playSpeed = speed;
    }
    public void playBackward (float speed) {
        playSpeed = -speed;
    }



}