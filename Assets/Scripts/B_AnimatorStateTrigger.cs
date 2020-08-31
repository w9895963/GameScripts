using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class B_AnimatorStateTrigger : StateMachineBehaviour {
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Main (animator, stateInfo, (e) => e.OnStateEnter.Invoke ());
    }



    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Main (animator, stateInfo, (e) => e.OnStateExist.Invoke ());
    }
    private static void Main (
        Animator animator,
        AnimatorStateInfo stateInfo,
        UnityAction<M_AnimatorStateEvent.StateEvent> action) {


        M_AnimatorStateEvent eventCom = animator.GetComponent<M_AnimatorStateEvent> ();
        M_AnimatorStateEvent.StateEvent[] events = eventCom.stateEvents;
        if (eventCom.enabled) {
            foreach (var ev in events) {
                if (stateInfo.IsName (ev.name)) {
                    action (ev);
                    // ev.OnStateEnter.Invoke ();
                }

            }
        }
    }
}