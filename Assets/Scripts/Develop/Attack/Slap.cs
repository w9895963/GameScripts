using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;

public class Slap : MonoBehaviour
{
    public enum AnimationName { Stop, Run }
    public Data data = new Data();
    [System.Serializable]
    public class Data
    {

    }
    public Event events = new Event();
    public class Event
    {

    }



    private void RunSlayAnimation()
    {
        string runState = AnimationName.Run.ToString();
        string stopState = AnimationName.Stop.ToString();
        Animator animator = GetComponent<Animator>();
        Debug.Log(animator);
        Debug.Log(animator.gameObject);
        Debug.Log(animator.gameObject.activeSelf);
        animator.Play(runState);
        animator.SetBool("Bool", true);
        // animator.GetCurrentAnimatorStateInfo().
        // Timer.Wait(gameObject, length, () =>
        // {
        //     animator.Play(stopState, 0, 0);
        // });


    }

    public void DoSlap()
    {
        RunSlayAnimation();

    }


}
