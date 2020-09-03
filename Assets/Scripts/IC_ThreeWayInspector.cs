using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_ThreeWayInspector : IC_Base {
    [System.Serializable] private class BehaviourView {
        public IC_Base[] next = new IC_Base[0];
        public IC_Base[] startWith = new IC_Base[0];
        public IC_Base[] endWith = new IC_Base[0];
    }

    [SerializeField] private BehaviourView Behaviour = new BehaviourView ();


    private void OnValidate () {
        Behaviour.startWith.ForEach ((x) => {
            x.behaviour.onStart.setEnable.AddNotHas (this);
        });
        Behaviour.endWith.ForEach ((x) => {
            x.behaviour.onFinish.ExpendTo (0) [0].setDisable.AddNotHas (this);
        });

        behaviour.onFinish.ExpendTo (0) [0].setEnable = Behaviour.next.ToList ();
    }
}